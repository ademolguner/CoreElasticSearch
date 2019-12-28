using MyBlogProject.Core.Business.EntityRepository;
using MyBlogProject.Entities.Models;
using System.Collections.Generic;

namespace MyBlogProject.Business.Abstract
{
    public interface ICommentService : IEntityCommonRepository<Comment>
    {
        List<Comment> GetCommentsByPostId(int postId);

        List<Comment> GetCommentsByPostId(int postId, int lastAmount);
    }
}