using  MyBlogProject.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace  MyBlogProject.Entities.Models
{
    public class Comment : IEntity
    {
        public int CommentId { get; set; }
        public string CommentContent { get; set; }
        public DateTime CommentDate { get; set; }
        public string GuestFullName { get; set; }
        public bool IsDelete { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
