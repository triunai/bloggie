using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories.Interface
{
  public interface IBlogPostRepository
  {
    // define 5 methods, create, getAll, get, update, delete
    Task<BlogPost?> CreateBlogPostAsync(BlogPost blogpost);

    // use IEnumerable data type to ensure data is read only, and only executed once it iterates over everything
    Task<IEnumerable<BlogPost>> GetAllBlogpostsAsync();
    Task<BlogPost?> GetBlogPostById(Guid Id); // not using the model or dto cuz we want a singular response based on its ID
    Task<BlogPost?> UpdateBlogpostAsync(BlogPost newBlogpost);
    
    // only need id, it will return a model as well for additional confirmation, check if it leaks
    Task<BlogPost?> DeleteBlogpostAsync(Guid Id);
  }
}
