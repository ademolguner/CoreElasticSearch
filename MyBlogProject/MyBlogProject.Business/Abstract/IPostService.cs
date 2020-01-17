using MyBlogProject.Business.ObjectDtos.Post;
using MyBlogProject.Core.Business.EntityRepository;
using MyBlogProject.Entities.ComplexTypes;
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
        PostDetailInfo GetPostDetail(int postId);
        Task<bool> PostAddOrUpdateElasticIndex(PostElasticIndexDto postElasticIndexDto);
        Task<bool> PostDeleteDocumentElasticIndex(PostElasticIndexDto postElasticIndexDto);
        Task<List<PostElasticIndexDto>> SuggestSearchAsync(string suggestText, int skipItemCount = 0, int maxItemCount = 100);
        Task<List<PostElasticIndexDto>> GetSearchAsync(string searchText,   int skipItemCount = 0, int maxItemCount = 100);
    }
}