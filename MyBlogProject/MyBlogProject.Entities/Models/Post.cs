using  MyBlogProject.Entites.Models;
using  MyBlogProject.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace  MyBlogProject.Entities.Models
{
    public class Post : IEntity
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        public int ReadCount { get; set; }
        public Category Category { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
