using  MyBlogProject.Core.DataAccess.EntityFramework;
using  MyBlogProject.DataAccess.Abstract;
using  MyBlogProject.DataAccess.Concrete;
using  MyBlogProject.Entities.Models;

namespace  MyBlogProject.DataAccess.Concrete.EntityFramework
{
    public class PostTagDal : EntityRepositoryBase<PostTag, AdemBlogDbContext>, IPostTagDal
    {
    }
}
