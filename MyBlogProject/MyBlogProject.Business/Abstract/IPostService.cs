using MyBlogProject.Core.Business.EntityRepository;
using MyBlogProject.Entities.Models;
using System.Collections.Generic;

namespace MyBlogProject.Business.Abstract
{
    public interface IPostService : IEntityCommonRepository<Post>
    {
        List<Post> GetPostsByCategoryId(int categoryId);
        List<Post> GetPostsByUserId(int userId);
        List<Post> GetPostsByCategoryId(int categoryId, int lastAmount);
    }
}