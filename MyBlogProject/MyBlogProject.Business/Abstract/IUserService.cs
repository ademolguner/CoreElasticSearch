using MyBlogProject.Core.Business.EntityRepository;
using MyBlogProject.Entities.Models;

namespace MyBlogProject.Business.Abstract
{
    public interface IUserService : IEntityCommonRepository<User>
    {
    }
}