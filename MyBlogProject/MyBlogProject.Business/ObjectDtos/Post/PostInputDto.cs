using MyBlogProject.Core.Dtos;
using MyBlogProject.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlogProject.Business.ObjectDtos
{
    public class PostInputDto : IDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        // public DateTime CreatedDate { get { return createdDate; } }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public int ReadCount { get; set; }
        public List<int> PostTagIds { get; set; }
    }
}
