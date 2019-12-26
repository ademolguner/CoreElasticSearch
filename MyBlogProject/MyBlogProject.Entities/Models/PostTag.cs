using  MyBlogProject.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace  MyBlogProject.Entities.Models
{
    public class PostTag : IEntity
    {
        public int PostTagId { get; set; }
        public int PostId { get; set; }
        public int TagId { get; set; }
    }
}
