using  MyBlogProject.Entities.Models;
using  MyBlogProject.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace  MyBlogProject.Entities.ComplexTypes
{
   public class PostTagDto : IEntity
    {
        public string TagValueName { get; set; }
    }
}
