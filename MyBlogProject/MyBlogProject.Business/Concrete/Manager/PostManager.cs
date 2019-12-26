using  MyBlogProject.Business.Abstract;
using  MyBlogProject.Business.ElasticSearchOptions;
using  MyBlogProject.Business.ObjectDtos.Post;
using  MyBlogProject.Core.Consts;
using  MyBlogProject.DataAccess.Abstract;
using  MyBlogProject.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace  MyBlogProject.Business.Concrete.Manager
{
    public class PostManager : IPostService
    {
        private readonly IPostDal _postDal;
        private readonly IElasticSearchService _elasticSearchService;
        private readonly ITagDal _tagDal;
        public PostManager(IPostDal postDal, IElasticSearchService elasticSearchService, ITagDal tagDal)
        {
            _postDal = postDal;
            _elasticSearchService = elasticSearchService;
            _tagDal = tagDal;
        }
        public Post Add(Post post)
        {
            _postDal.Add(post);
            // elasticsearch işlemi
            _elasticSearchService.AddOrUpdateAsync<PostElasticIndexDto, Guid>(ElasticSearchItemsConst.PostIndexName, new PostElasticIndexDto()
            {
                Id = Guid.NewGuid(),
                Title = post.Title,
                PostContent = post.Content,
                CategoryName = post.Category.CategoryName,
                Url = "/post/id/" + post.PostId.ToString(),
                TagNameValues = _tagDal.GetPostTags(post.PostId).Select(x => x.TagValueName).ToList()
            });

            return post;
        }
        public void Delete(Post post)
        {
            _postDal.Delete(post);
        }
        public List<Post> GetAll()
        {
            return _postDal.GetList();
        }
        public Post GetById(int id)
        {
            return _postDal.Get(c => c.PostId == id);
        }
        public Post Update(Post post)
        {
            _postDal.Update(post);
            return GetById(post.PostId);
        }
        public List<Post> GetPostsByCategoryId(int categoryId)
        {
            return _postDal.GetList(c => c.CategoryId == categoryId);
        }
        public List<Post> GetPostsByCategoryId(int categoryId, int lastAmount)
        {
            return _postDal.GetList(c => c.CategoryId == categoryId).Take(lastAmount).ToList();
        }
    }
}
