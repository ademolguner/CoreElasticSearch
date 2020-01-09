using MyBlogProject.Core.Entities;

namespace MyBlogProject.Entities.Models
{
    public class User : IEntity
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName
        {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }
    }
}