using MyBlogProject.Business.Abstract;
using MyBlogProject.DataAccess.Abstract;
using MyBlogProject.Entities.ComplexTypes;
using MyBlogProject.Entities.Models;
using System.Collections.Generic;

namespace MyBlogProject.Business.Concrete.Manager
{
    public class TagManager : ITagService
    {
        private readonly ITagDal _tagDal;

        public TagManager(ITagDal tagDal)
        {
            _tagDal = tagDal;
        }

        public Tag Insert(Tag tag)
        {
            _tagDal.Add(tag);
            return tag;
        }

        public void Remove(Tag tag)
        {
            _tagDal.Delete(tag);
        }

        public List<Tag> GetAllList()
        {
            return _tagDal.GetList();
        }

        public Tag GetByItem(object item)
        {
            return _tagDal.Get(c => c.TagId == (int)item);
        }

        public Tag Update(Tag tag)
        {
            _tagDal.Update(tag);
            return tag;
        }

        public List<PostTagsInfo> PostTagListForPost(int postID)
        {
            return _tagDal.GetPostTags(postID);
        }
    }
}