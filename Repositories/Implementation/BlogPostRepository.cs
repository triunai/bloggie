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
    public async Task<BlogPost?> CreateBlogPostAsync(BlogPost blogpost)
    {
      await dbContext.BlogPost.AddAsync(blogpost); //method to add into database
      await dbContext.SaveChangesAsync(); //saving changes in databsae
      return blogpost;
    }


    public async Task<IEnumerable<BlogPost>> GetAllBlogpostsAsync()
    {
      return await dbContext.BlogPost.Include( x => x.Categories ).ToListAsync();
    }

    public async Task<BlogPost?> GetBlogPostById(Guid Id)
    {
      return await dbContext.BlogPost.Include( x => x.Categories ).FirstOrDefaultAsync( x => x.Id == Id);
    }

    public async Task<BlogPost?> UpdateBlogpostAsync(BlogPost newBlogpost)
    {
       var matchingBlogFromDb = await dbContext.BlogPost.Include(x => x.Categories).FirstOrDefaultAsync( data => data.Id == newBlogpost.Id );
       if (matchingBlogFromDb != null)
        {

        // basically we take the id we wanna change, and then set its new value using the current values attribute
        dbContext.Entry(matchingBlogFromDb).CurrentValues.SetValues(newBlogpost); 
        // saving categories next
        matchingBlogFromDb.Categories = newBlogpost.Categories;

        await dbContext.SaveChangesAsync();
        return matchingBlogFromDb;

      }
      return null;
    }

    public async Task<BlogPost?> DeleteBlogpostAsync(Guid Id)
    {
      try
      {
        // will query faster if you remove "Include"
        var existingBlogpost = await dbContext.BlogPost.Include(x => x.Categories).FirstOrDefaultAsync(bp => bp.Id == Id);

        if (existingBlogpost == null)
        {
          return null;
        }

        dbContext.BlogPost.Remove(existingBlogpost);
        await dbContext.SaveChangesAsync();
        return existingBlogpost;
      }
      catch (Exception ex)
      {
        // Log the exception
        return null;
      }

    }
  }
}
