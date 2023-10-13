using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
  public class BlogPostRepository : IBlogPostRepository
  {

    // declare dbContext to perform db operations first and then inject it into ctor
    private readonly ApplicationDbContext dbContext;

    public BlogPostRepository( ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public async Task<BlogPost> CreateBlogPostAsync(BlogPost blogpost)
    {
      await dbContext.BlogPost.AddAsync(blogpost); //method to add into database
      await dbContext.SaveChangesAsync(); //saving changes in databsae
      return blogpost;
    }

    public async Task<IEnumerable<BlogPost>> GetAllBlogpostsAsync()
    {
      return await dbContext.BlogPost.ToListAsync();
    }
  }
}
