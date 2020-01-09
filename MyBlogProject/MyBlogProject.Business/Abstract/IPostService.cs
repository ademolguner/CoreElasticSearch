using MyBlogProject.Business.ObjectDtos.Post;
using MyBlogProject.Core.Business.EntityRepository;
using MyBlogProject.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBlogProject.Business.Abstract
{
    public interface IPostService : IEntityCommonRepository<Post>
    {
        List<Post> GetPostsByCategoryId(int categoryId);
        List<Post> GetPostsByUserId(int userId);
        List<Post> GetPostsByCategoryId(int categoryId, int lastAmount);
        //
        Task<bool> PostAddOrUpdateElasticIndex(PostElasticIndexDto postElasticIndexDto);
        Task<bool> PostDeleteDocumentElasticIndex(PostElasticIndexDto postElasticIndexDto);
    }
}