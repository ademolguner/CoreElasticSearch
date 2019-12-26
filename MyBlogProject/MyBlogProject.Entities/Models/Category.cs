using  MyBlogProject.Core.Entities;
using System.Collections.Generic;

namespace  MyBlogProject.Entites.Models
{
    public class Category : IEntity
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}