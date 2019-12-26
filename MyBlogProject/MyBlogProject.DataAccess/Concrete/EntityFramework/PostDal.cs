using  MyBlogProject.Core.DataAccess.EntityFramework;
using  MyBlogProject.DataAccess.Abstract;
using  MyBlogProject.DataAccess.Concrete;
using  MyBlogProject.Entities.Models;

namespace  MyBlogProject.DataAccess.Concrete.EntityFramework
{
    public class PostDal : EntityRepositoryBase<Post, AdemBlogDbContext>, IPostDal
    {
    }
}
