using CodePulse.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<BlogPost> BlogPost { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<BlogImage> BlogImages { get; set; } 
    }
}
