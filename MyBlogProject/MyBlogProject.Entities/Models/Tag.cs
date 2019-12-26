using  MyBlogProject.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace  MyBlogProject.Entities.Models
{
    public class Tag : IEntity
    {
        public int TagId { get; set; }
        public string TagName { get; set; }

    }
}
