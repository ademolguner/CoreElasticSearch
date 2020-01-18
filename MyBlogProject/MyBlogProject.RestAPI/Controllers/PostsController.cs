using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyBlogProject.Business.Abstract;
using MyBlogProject.Business.ElasticSearchOptions;
using MyBlogProject.Business.ObjectDtos;
using MyBlogProject.Business.ObjectDtos.Post;
using MyBlogProject.Core.Consts;
using MyBlogProject.Entities.Models;
using MyBlogProject.RestAPI.Helpers;

namespace MyBlogProject.RestAPI.Controllers
{
    //[ServiceFilter(typeof(TokenFilter))]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]

    public class PostsController : ControllerBase
    {
        private readonly ILogger<PostsController> _logger;
        private readonly IPostService _postService;
        private readonly ICategoryService _categoryService;
        private readonly IPostTagService _PostTagService;
        private readonly IUserService _userService;
        private readonly ITagService _tagService;
        private readonly IMapper _mapper;

        public PostsController(IMapper mapper, ITagService tagService, IUserService userService, ICategoryService categoryService, ILogger<PostsController> logger, IPostService postService, IPostTagService PostTagService)
        {
            _logger = logger;
            _postService = postService;
            _PostTagService = PostTagService;
            _categoryService = categoryService;
            _userService = userService;
            _tagService = tagService;
            _mapper = mapper;
        }

        [HttpGet("HepsiniGetirelim")]
        public async Task<List<Post>> Get()
        {
             



            return await Task.FromResult(_postService.GetAllList());
        }

        [HttpGet("IndexleriYenile")]
        public async void ReIndexAll()
        {
            var allList = _postService.GetAllList();
            foreach (var item in allList)
            {
                var itemInfo = await Task.FromResult(_postService.GetPostDetail(item.PostId));
                var suggestList = GetPostElasticSuggetItems(item);
                var tagNameValues = GetPostTagListForPost(item);

                var indexCreateItem = GetElasticIndexItem(item, tagNameValues, itemInfo.TagIds, suggestList);
                PostAddOrUpdateElasticIndex(indexCreateItem);
                 
            }

        }


        [HttpPut("DegerleriniGuncelleyelim")]
        public async Task<Post> Update(PostInputDto postInputDto)
        {
            //using (TransactionScope scope = new TransactionScope())
            //{
            try
            {
                var updatedPostInfo = _postService.Update(_mapper.Map<Post>(postInputDto));
                PostTagsRemove(updatedPostInfo.PostId);
                PostTagAdded(postInputDto, updatedPostInfo.PostId);
                var suggestList = GetPostElasticSuggetItems(updatedPostInfo);
                var tagNameValues = GetPostTagListForPost(updatedPostInfo);
                var indexCreateItem = GetElasticIndexItem(updatedPostInfo, tagNameValues, postInputDto.PostTagIds, suggestList);
                PostAddOrUpdateElasticIndex(indexCreateItem);
                // scope.Complete();
                return await Task.FromResult(updatedPostInfo);
            }
            catch (Exception ex)
            {
                // scope.Dispose();
                _logger.LogError(ex.Message, ex);
                return await Task.FromException<Post>(ex);
            }
            //}          
        }


        [HttpPost("YeniOlsturalim")]
        public async Task<Post> AddNewPost(PostInputDto postInputDto)
        {
            try
            {
                // Yeni bir Post ekliyoruz (Insert işlemi)
                // _mapper AutoMapper yardımı ile gelen (PostInputDto postInputDto) nesnesi Post tipine mapliyoruz
                var CreatedPostInfo = _postService.Insert(_mapper.Map<Post>(postInputDto));
                // PostTag tablosuna PostId ve TagId (birden çok olabilir) parametreleri ile kayıt atıyoruz
                PostTagAdded(postInputDto, CreatedPostInfo.PostId);
                /* Indexleme işleminde önerme (Suggest) işlemi için keywordler oluşturuyoruz
                 Burada keyword olarak CategoryName, ve eklenen TagName (birden çok olabilir) bilgilerini alıyoruz isterseniz Post'un başlık alanınıda ekleyebiliriz.
                 Önerme (Suggest) işlemlerinde hangi keywordlerden yararlanmak istiyorsak ekleyebiliriz.    */
                var suggestList = GetPostElasticSuggetItems(CreatedPostInfo);
                // Indexleme işleminde TagNameValues alanı olarak TagName degerlerini bir List<string> liste olarak alıyoruz.
                var tagNameValues = GetPostTagListForPost(CreatedPostInfo);
                // Indexleme işlemi için son olarak elde ettiğimiz tüm veriler ile Index atılacak nesnemizi (PostElasticIndexDto) oluşturuyoruz.
                var indexCreateItem = GetElasticIndexItem(CreatedPostInfo, tagNameValues, postInputDto.PostTagIds, suggestList);
                // Index ekleme işlemi için yazılan metot
                PostAddOrUpdateElasticIndex(indexCreateItem);
                // Geriye, oluşturulan nesnemizi (PostElasticIndexDto) dönüyoruz.
                return await Task.FromResult(CreatedPostInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return await Task.FromException<Post>(ex);
            }
        }



        [HttpDelete("SilelimKaybedelim")]
        public async Task<bool> DeletePostAsync(int postId)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    var postTagList = _PostTagService.GetListByPostId(postId);
                    foreach (var item in postTagList)
                    {
                        _PostTagService.Remove(item);
                    }
                    var postInfo = _postService.GetByItem(postId);
                    _postService.Remove(postInfo);

                    var status = _postService.PostDeleteDocumentElasticIndex(new PostElasticIndexDto
                    {
                        Id = postId
                    });

                    scope.Complete();
                    return await Task.FromResult<bool>(status.Result);
                }
                catch
                {
                    return await Task.FromResult<bool>(false);
                }
            }
        }

        [HttpGet("OnermeYapalim_AutoSuggest")]
        public async Task<List<PostElasticIndexDto>> GetPostSuggestSearchAsync(string suggestText)
        {
             return await Task.FromResult(_postService.SuggestSearchAsync(suggestText,0,10).Result);
        }

        [HttpGet("AraBakalim")]
        public async Task<List<PostElasticIndexDto>> GetSearchAsync(string searchtext)
        {
            //return await Task.FromResult(_postService.GetSearchAsync(searchtext,0,100).Result);
            return await Task.FromResult(_postService.GetSearchAsync(searchtext).Result);
        }
























        #region Yardımcı metotlar
        [ApiExplorerSettings(IgnoreApi = true)]
        private async void PostAddOrUpdateElasticIndex(PostElasticIndexDto postElastic)
        {
            await _postService.PostAddOrUpdateElasticIndex(postElastic);
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        private void PostTagsRemove(int postId)
        {
            _PostTagService.RemoveAllPostTagsByPostId(postId);
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        private void PostTagAdded(PostInputDto postInputDto, int postId)
        {
            foreach (var itemTag in postInputDto.PostTagIds)
                _PostTagService.Insert(new PostTag { PostId = postId, TagId = itemTag });
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        private List<string> GetPostTagListForPost(Post postInfo)
        {
            return _tagService.PostTagListForPost(postInfo.PostId).Select(x => x.TagValueName).ToList();
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        private List<string> GetPostElasticSuggetItems(Post postInfo)
        {

            var suggestList = new List<string>();
            var tagNameValues = GetPostTagListForPost(postInfo);
            suggestList.AddRange(tagNameValues);
            suggestList.Add(postInfo.Title);
            return suggestList;
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        private PostElasticIndexDto GetElasticIndexItem(Post postInfo, List<string> tagNameValues, List<int> postTagIds , List<string> suggestList)
        {
            return new PostElasticIndexDto
            {
                CategoryId = postInfo.CategoryId,
                CategoryName = _categoryService.GetByItem(postInfo.CategoryId).CategoryName,
                Title = postInfo.Title,
                UserInfo = _userService.GetByItem(postInfo.UserId).FullName,
                TagNameValues = tagNameValues,
                TagNameIds = postTagIds,
                Suggest = new Nest.CompletionField { Input = suggestList.Distinct() },
                SearchingArea = postInfo.Content,
                Url = "api/posts/" + postInfo.PostId.ToString(),
                Id = postInfo.PostId,
                CreatedDate = DateTime.Now,
                UserId = postInfo.UserId
            };
        }
        #endregion
    }
}
