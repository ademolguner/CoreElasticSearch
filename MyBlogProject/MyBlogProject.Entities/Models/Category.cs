using MyBlogProject.Core.Entities;

namespace MyBlogProject.Entites.Models
{
    public class Category : IEntity
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}