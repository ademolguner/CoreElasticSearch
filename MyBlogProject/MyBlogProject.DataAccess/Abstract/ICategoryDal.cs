using MyBlogProject.Core.DataAccess;
using MyBlogProject.Entites.Models;

namespace MyBlogProject.DataAccess.Abstract
{
    public interface ICategoryDal : IEntityRepository<Category>
    {
    }
}