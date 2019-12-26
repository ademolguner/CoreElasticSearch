using  MyBlogProject.Core.DataAccess;
using  MyBlogProject.Entities.Models;
using  MyBlogProject.Entities.ComplexTypes;
using System.Collections.Generic;

namespace  MyBlogProject.DataAccess.Abstract
{

    public interface ITagDal : IEntityRepository<Tag>
    {
        List<PostTagDto> GetPostTags(int postId);
    }
}

