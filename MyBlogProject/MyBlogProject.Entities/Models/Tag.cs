using MyBlogProject.Core.Entities;

namespace MyBlogProject.Entities.Models
{
    public class Tag : IEntity
    {
        public int TagId { get; set; }
        public string TagName { get; set; }
    }
}