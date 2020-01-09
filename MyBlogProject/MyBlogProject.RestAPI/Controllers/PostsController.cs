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
    //[Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]

    public class PostsController : ControllerBase
    {
        private readonly ILogger<PostsController> _logger;
        private readonly IPostService _postService;
        private readonly ICategoryService _categoryService;
        private readonly IPostTagService _PostTagService;
        private readonly IElasticSearchService _elasticSearchService;
        private readonly IUserService _userService;
        private readonly ITagService _tagService;
        private readonly IMapper _mapper;

        public PostsController(IMapper mapper, ITagService tagService, IUserService userService, ICategoryService categoryService, ILogger<PostsController> logger, IPostService postService, IPostTagService PostTagService, IElasticSearchService elasticSearchService)
        {
            _logger = logger;
            _postService = postService;
            _PostTagService = PostTagService;
            _elasticSearchService = elasticSearchService;
            _categoryService = categoryService;
            _userService = userService;
            _tagService = tagService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<List<Post>> Get()
        {
            return await Task.FromResult(_postService.GetAllList());
        }


        [HttpPut]
        public async Task<Post> Update(PostInputDto postInputDto)
        {
            //using (TransactionScope scope = new TransactionScope())
            //{
            try
            {
                var postInfo = _postService.Update(_mapper.Map<Post>(postInputDto));
                PostTagsRemove(postInfo.PostId);
                PostTagAdded(postInputDto, postInfo.PostId);
                var suggestList = GetPostElasticSuggetItems(postInfo);
                var tagNameValues = GetPostTagListForPost(postInfo);
                PostAddOrUpdateElasticIndex(new PostElasticIndexDto
                {
                    CategoryName = _categoryService.GetByItem(postInfo.CategoryId).CategoryName,
                    PostContent = postInfo.Content,
                    Title = postInfo.Title,
                    UserInfo = _userService.GetByItem(postInfo.UserId).FullName,
                    TagNameValues = tagNameValues,
                    Suggest = new Nest.CompletionField { Input = suggestList.Distinct() },
                    SearchableText = postInfo.Title + " " + postInfo.Content,
                    Url = "api/posts/" + postInfo.PostId.ToString(),
                    Id = postInfo.PostId
                });
                // scope.Complete();
                return await Task.FromResult(postInfo);
            }
            catch (Exception ex)
            {
                // scope.Dispose();
                _logger.LogError(ex.Message, ex);
                return await Task.FromException<Post>(ex);
            }
            //}          
        }


        [HttpPost]
        public async Task<Post> AddNewPost([FromBody] PostInputDto postInputDto)
        {
            //using (TransactionScope scope = new TransactionScope())
            //{
            try
            {
                var postInfo = _postService.Insert(_mapper.Map<Post>(postInputDto));
                PostTagAdded(postInputDto, postInfo.PostId);
                var suggestList = GetPostElasticSuggetItems(postInfo);
                var tagNameValues = GetPostTagListForPost(postInfo);
                PostAddOrUpdateElasticIndex(new PostElasticIndexDto
                {
                    CategoryName = _categoryService.GetByItem(postInfo.CategoryId).CategoryName,
                    PostContent = postInfo.Content,
                    Title = postInfo.Title,
                    UserInfo = _userService.GetByItem(postInfo.UserId).FullName,
                    TagNameValues = tagNameValues,
                    Suggest = new Nest.CompletionField { Input = suggestList.Distinct() },
                    SearchableText = postInfo.Title + " " + postInfo.Content,
                    Url = "api/posts/" + postInfo.PostId.ToString(),
                    Id = postInfo.PostId
                });
                // scope.Complete();
                return await Task.FromResult(postInfo);
            }
            catch (Exception ex)
            {
                // scope.Dispose();
                _logger.LogError(ex.Message, ex);
                return await Task.FromException<Post>(ex);
            }
            //}
        }

        [HttpDelete]
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
                catch (Exception ex)
                {
                    return await Task.FromResult<bool>(false);
                }


            }
        }



        private async void PostAddOrUpdateElasticIndex(PostElasticIndexDto postElastic)
        {
            await _postService.PostAddOrUpdateElasticIndex(postElastic);
        }

        private void PostTagsRemove(int postId)
        {
            _PostTagService.RemoveAllPostTagsByPostId(postId);
        }
        private void PostTagAdded(PostInputDto postInputDto, int postId)
        {
            foreach (var itemTag in postInputDto.PostTagIds)
                _PostTagService.Insert(new PostTag { PostId = postId, TagId = itemTag });
        }

        private List<string> GetPostTagListForPost(Post postInfo)
        {
            return _tagService.PostTagListForPost(postInfo.PostId).Select(x => x.TagValueName).ToList();
        }
        private List<string> GetPostElasticSuggetItems(Post postInfo)
        {

            var suggestList = new List<string>();
            var tagNameValues = GetPostTagListForPost(postInfo);
            suggestList.AddRange(tagNameValues);
            suggestList.Add(postInfo.Title);
            return suggestList;
        }
    }
}
