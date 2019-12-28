using MyBlogProject.Core.Entities;

namespace MyBlogProject.Entities.ComplexTypes
{
    public class PostTagDto : IEntity
    {
        public string TagValueName { get; set; }
    }
}