using MyBlogProject.Core.Entities;

namespace MyBlogProject.Entities.ComplexTypes
{
    public class PostTagsInfo : IEntity
    {
        public string TagValueName { get; set; }
        public int TagValueId { get; set; }
    }
}