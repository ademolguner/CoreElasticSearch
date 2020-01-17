using MyBlogProject.Core.Entities;
using MyBlogProject.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlogProject.Entities.ComplexTypes
{
   public  class PostDetailInfo:IEntity
    {
        public Post PostInfo { get; set; }
        public string UserName { get; set; }
        public string CategoryName { get; set; }
        public List<int> TagIds { get; set; }

    }
}
