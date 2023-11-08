using CodePulse.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        // business domain models
        public DbSet<BlogPost> BlogPost { get; set; } // communicates with blogpost table
        public DbSet<Categories> Categories { get; set; } // communicates with categories table
        public DbSet<BlogImage> BlogImages { get; set; } // communicates with blogimages
    } 
}
