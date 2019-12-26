using  MyBlogProject.Business.Abstract;
using  MyBlogProject.DataAccess.Abstract;
using  MyBlogProject.Entities.Models;
using System.Collections.Generic;

namespace  MyBlogProject.Business.Concrete.Manager
{
    public class PostTagManager : IPostTagService
    {

        private readonly IPostTagDal _postTagDal;
        public PostTagManager(IPostTagDal postTagDal)
        {
            _postTagDal = postTagDal;
        }
        public PostTag Add(PostTag postTag)
        {
            _postTagDal.Add(postTag);
            return postTag;
        }

        public void Delete(PostTag postTag)
        {
            _postTagDal.Delete(postTag);
        }

        public List<PostTag> GetAll()
        {
            return _postTagDal.GetList();
        }

        public PostTag GetById(int id)
        {
            return _postTagDal.Get(c => c.PostTagId == id);
        }

        public List<PostTag> GetListByPostId(int postId)
        {
            return _postTagDal.GetList(c => c.PostId==postId);
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
    }
}
