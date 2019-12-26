using  MyBlogProject.Entities.Models;
using System.Collections.Generic;

namespace  MyBlogProject.Business.Abstract
{
    public interface ICommentService
    {
        List<Comment> GetAll();

        Comment GetById(int id);

        Comment Add(Comment comment);

        Comment Update(Comment comment);

        void Delete(Comment comment);

        List<Comment> GetCommentsByPostId(int postId);
        List<Comment> GetCommentsByPostId(int postId, int lastAmount);

        
    }
}
