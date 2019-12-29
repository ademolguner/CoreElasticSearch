using MyBlogProject.Core.DataAccess.EntityFramework;
using MyBlogProject.DataAccess.Abstract;
using MyBlogProject.Entities.ComplexTypes;
using MyBlogProject.Entities.Models;
using System.Linq;

namespace MyBlogProject.DataAccess.Concrete.EntityFramework
{
    public class PostDal : EntityRepositoryBase<Post, AdemBlogDbContext>, IPostDal
    {
        public PostDetailInfo GetPostDetail(int postId)
        {
            using (var dbContext= new AdemBlogDbContext())
            {
                var data = from p in dbContext.Post
                           join c in dbContext.Category
                           on p.CategoryId equals c.CategoryId
                           join u in dbContext.User
                           on p.UserId equals u.UserId
                           where p.PostId == postId
                           select new PostDetailInfo
                           {
                               PostInfo = p,
                               CategoryName = c.CategoryName,
                               UserName = u.FullName
                           };
                return data?.FirstOrDefault();
            }
        }
    }
}