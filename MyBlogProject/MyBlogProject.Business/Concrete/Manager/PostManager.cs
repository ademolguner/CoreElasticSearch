using MyBlogProject.Business.Abstract;
using MyBlogProject.Business.ElasticSearchOptions;
using MyBlogProject.Business.ObjectDtos.Post;
using MyBlogProject.Core.Consts;
using MyBlogProject.DataAccess.Abstract;
using MyBlogProject.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace MyBlogProject.Business.Concrete.Manager
{
    public class PostManager : IPostService
    {
        private readonly IPostDal _postDal;
        private readonly IElasticSearchService _elasticSearchService;
        private readonly ITagService _tagService;
        private readonly IUserService _userService;

        public PostManager(IPostDal postDal, IElasticSearchService elasticSearchService, ITagService tagService, IUserService userService)
        {
            _postDal = postDal;
            _elasticSearchService = elasticSearchService;
            _tagService = tagService;
            _userService = userService;
        }

        public Post Insert(Post post)
        {
            _postDal.Add(post);
            return post;
        }

        public void Remove(Post post)
        {
            _postDal.Delete(post);
        }

        public List<Post> GetAllList()
        {
            return _postDal.GetList();
        }

        public Post GetByItem(object item)
        {
            return _postDal.Get(c => c.PostId == (int)item);
        }

        public Post Update(Post post)
        {
            _postDal.Update(post);
            return GetByItem(post.PostId);
        }

        public List<Post> GetPostsByCategoryId(int categoryId)
        {
            return _postDal.GetList(c => c.CategoryId == categoryId);
        }

        public List<Post> GetPostsByCategoryId(int categoryId, int lastAmount)
        {
            return _postDal.GetList(c => c.CategoryId == categoryId).Take(lastAmount).ToList();
        }

        public List<Post> GetPostsByUserId(int userId)
        {
            return _postDal.GetList(c => c.UserId == userId);
        }

        public async Task<bool> PostAddOrUpdateElasticIndex(PostElasticIndexDto postElasticIndexDto)
        {
            try
            {
                await _elasticSearchService.CreateIndexAsync<PostElasticIndexDto, int>(ElasticSearchItemsConst.PostIndexName);
                await _elasticSearchService.AddOrUpdateAsync<PostElasticIndexDto, int>(
                    ElasticSearchItemsConst.PostIndexName,
                    postElasticIndexDto);
                return true;
            }
            catch (Exception)
            {
                return false;
            } 
        }

        public async  Task<bool> PostDeleteDocumentElasticIndex(PostElasticIndexDto postElasticIndexDto)
        {
            try
            {
                await _elasticSearchService.DeleteAsync<PostElasticIndexDto, int>(
                    ElasticSearchItemsConst.PostIndexName,
                    "postelasticindexdto",
                    postElasticIndexDto
                    );
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}