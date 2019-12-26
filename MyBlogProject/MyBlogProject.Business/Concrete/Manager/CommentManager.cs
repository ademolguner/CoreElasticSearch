using  MyBlogProject.Business.Abstract;
using  MyBlogProject.DataAccess.Abstract;
using  MyBlogProject.Entities.Models;
using System.Collections.Generic;
using System.Linq;

namespace  MyBlogProject.Business.Concrete.Manager
{
    public class CommentManager : ICommentService
    {
        private readonly ICommentDal _commentDal;

        public CommentManager(ICommentDal commentDal)
        {
            _commentDal = commentDal;
        }

        public Comment Add(Comment comment)
        {
            _commentDal.Add(comment);
            return comment;
        }

        public void Delete(Comment comment)
        {
            _commentDal.Delete(comment); 
        }

        public List<Comment> GetAll()
        {
            return _commentDal.GetList();
        }

        public Comment GetById(int id)
        {
            return _commentDal.Get(c => c.CommentId == id);
        }

        public List<Comment> GetCommentsByPostId(int postId)
        {
            return _commentDal.GetList(c => c.PostId == postId);
        }

        public List<Comment> GetCommentsByPostId(int postId, int lastAmount)
        {
              return _commentDal.GetList(c => c.PostId == postId)
                .OrderByDescending(x=> x.CommentDate)
                .Take(lastAmount)
                .ToList();
        }

        public Comment Update(Comment comment)
        {
            _commentDal.Update(comment);
            return GetById(comment.CommentId);
        }
    }
}
