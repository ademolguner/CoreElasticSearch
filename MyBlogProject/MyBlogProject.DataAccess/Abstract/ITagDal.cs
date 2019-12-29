using MyBlogProject.Core.DataAccess;
using MyBlogProject.Entities.ComplexTypes;
using MyBlogProject.Entities.Models;
using System.Collections.Generic;

namespace MyBlogProject.DataAccess.Abstract
{
    public interface ITagDal : IEntityRepository<Tag>
    {
        List<PostTagsInfo> GetPostTags(int postId);
    }
}