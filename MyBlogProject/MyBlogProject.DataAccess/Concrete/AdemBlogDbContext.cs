using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using  MyBlogProject.Entities.Models;
using  MyBlogProject.Entities.ComplexTypes;
using System.IO;
using  MyBlogProject.Entites.Models;

namespace  MyBlogProject.DataAccess.Concrete
{
    public class AdemBlogDbContext : DbContext
    {

        public AdemBlogDbContext()
        {
        }
        public AdemBlogDbContext(DbContextOptions<AdemBlogDbContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
            optionsBuilder.UseSqlServer(@"Data Source=.;Initial Catalog=AdemBlogDb;Integrated Security=True;");
        }

        public DbSet<Post> Post { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<PostTag> PostTag { get; set; } 
    }
}
