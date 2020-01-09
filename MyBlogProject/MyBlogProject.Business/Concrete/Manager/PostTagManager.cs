using MyBlogProject.Business.Abstract;
using MyBlogProject.DataAccess.Abstract;
using MyBlogProject.Entities.Models;
using System.Collections.Generic;

namespace MyBlogProject.Business.Concrete.Manager
{
    public class PostTagManager : IPostTagService
    {
        private readonly IPostTagDal _postTagDal;

        public PostTagManager(IPostTagDal postTagDal)
        {
            _postTagDal = postTagDal;
        }

        public PostTag Insert(PostTag postTag)
        {
            _postTagDal.Add(postTag);
            return postTag;
        }

        public void Remove(PostTag postTag)
        {
            _postTagDal.Delete(postTag);
        }

        public List<PostTag> GetAllList()
        {
            return _postTagDal.GetList();
        }

        public PostTag GetByItem(object item)
        {
            return _postTagDal.Get(c => c.PostTagId == (int)item);
        }

        public List<PostTag> GetListByPostId(int postId)
        {
            return _postTagDal.GetList(c => c.PostId == postId);
        }

        public List<PostTag> GetListByTagId(int tagId)
        {
            return _postTagDal.GetList(c => c.TagId == tagId);
        }

        public PostTag Update(PostTag postTag)
        {
            _postTagDal.Update(postTag);
            return postTag;
        }

        public void RemoveAllPostTagsByPostId(int postId)
        {
            var data = GetListByPostId(postId);
            foreach (var item in data)
            {
                Remove(item);
            }
        }
    }
}