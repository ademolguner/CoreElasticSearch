using MyBlogProject.Core.DataAccess.EntityFramework;
using MyBlogProject.DataAccess.Abstract;
using MyBlogProject.Entities.Models;

namespace MyBlogProject.DataAccess.Concrete.EntityFramework
{
    public class CommentDal : EntityRepositoryBase<Comment, AdemBlogDbContext>, ICommentDal
    {
    }
}