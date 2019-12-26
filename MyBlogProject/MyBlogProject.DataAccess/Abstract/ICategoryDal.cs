using  MyBlogProject.Core.DataAccess;
using  MyBlogProject.Entites.Models;
using  MyBlogProject.Entities.Models;

namespace  MyBlogProject.DataAccess.Abstract
{
    public interface ICategoryDal : IEntityRepository<Category>
    {
    }
}
