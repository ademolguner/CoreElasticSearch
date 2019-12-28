using MyBlogProject.Core.DataAccess;
using MyBlogProject.Entities.Models;

namespace MyBlogProject.DataAccess.Abstract
{
    public interface ICommentDal : IEntityRepository<Comment>
    {
    }
}