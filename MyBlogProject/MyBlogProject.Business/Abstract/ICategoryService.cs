using MyBlogProject.Core.Business.EntityRepository;
using MyBlogProject.Entites.Models;

namespace MyBlogProject.Business.Abstract
{
    public interface ICategoryService : IEntityCommonRepository<Category>
    {
    }
}