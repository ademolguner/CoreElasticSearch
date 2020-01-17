using MyBlogProject.Core.DataAccess.EntityFramework;
using MyBlogProject.DataAccess.Abstract;
using MyBlogProject.Entities.ComplexTypes;
using MyBlogProject.Entities.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyBlogProject.DataAccess.Concrete.EntityFramework
{
    public class TagDal : EntityRepositoryBase<Tag, AdemBlogDbContext>, ITagDal
    {
        public List<PostTagsInfo> GetPostTags(int postId)
        {
            using (AdemBlogDbContext context = new AdemBlogDbContext())
            {
                var tagNameList = from pt in context.PostTag
                                  join p in context.Post
                                  on pt.PostId equals p.PostId
                                  join t in context.Tag
                                  on pt.TagId equals t.TagId
                                  where p.PostId == postId
                                  select new PostTagsInfo { TagValueName = t.TagName, TagValueId=t.TagId };
                return tagNameList.ToList();
            }
        }
    }
}