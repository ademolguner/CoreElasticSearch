using  MyBlogProject.Core.DataAccess.EntityFramework;
using  MyBlogProject.DataAccess.Abstract;
using  MyBlogProject.DataAccess.Concrete;
using  MyBlogProject.Entites.Models;
using  MyBlogProject.Entities.Models;

namespace  MyBlogProject.DataAccess.Concrete.EntityFramework
{
    public class CategoryDal : EntityRepositoryBase<Category, AdemBlogDbContext>, ICategoryDal
    {
    }
}
