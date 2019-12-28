using MyBlogProject.Core.DataAccess.EntityFramework;
using MyBlogProject.DataAccess.Abstract;
using MyBlogProject.Entites.Models;

namespace MyBlogProject.DataAccess.Concrete.EntityFramework
{
    public class CategoryDal : EntityRepositoryBase<Category, AdemBlogDbContext>, ICategoryDal
    {
    }
}