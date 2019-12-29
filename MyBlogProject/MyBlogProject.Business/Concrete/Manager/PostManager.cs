using MyBlogProject.Business.Abstract;
using MyBlogProject.Business.ElasticSearchOptions;
using MyBlogProject.Business.ObjectDtos.Post;
using MyBlogProject.Core.Consts;
using MyBlogProject.DataAccess.Abstract;
using MyBlogProject.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public bool PostCreateElasticIndex(int postID)
        {
            var postInfo = _postDal.Get(c => c.PostId == postID);
            try
            {
                _elasticSearchService.AddOrUpdateAsync<PostElasticIndexDto, Guid>(
                   ElasticSearchItemsConst.PostIndexName,
                   new PostElasticIndexDto
                   {
                       Id = Guid.NewGuid(),
                       Title = postInfo.Title,
                       PostContent = postInfo.Content,
                       CategoryName = postInfo.Category.CategoryName,
                       Url = "/post/id/" + postInfo.PostId.ToString(),
                       TagNameValues = _tagService.PostTagListForPost(postInfo.PostId).Select(x => x.TagValueName).ToList(),
                       UserInfo = _userService.GetByItem(postInfo.UserId).FullName
                   });
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}