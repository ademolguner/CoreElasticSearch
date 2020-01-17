using MyBlogProject.Core.DataAccess;
using MyBlogProject.Entities.ComplexTypes;
using MyBlogProject.Entities.Models;

namespace MyBlogProject.DataAccess.Abstract
{
    public interface IPostDal : IEntityRepository<Post>
    {
        PostDetailInfo PostDetail(int postId);
    }
}