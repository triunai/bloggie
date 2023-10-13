using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories.Interface
{
  public interface IBlogPostRepository
  {
    // define 5 methods, create, getAll, get, update, delete
    Task<BlogPost?> CreateBlogPostAsync(BlogPost blogpost);

    // use IEnumerable data type to ensure data is read only, and only executed once it iterates over everything
    Task<IEnumerable<BlogPost>> GetAllBlogpostsAsync();
  }
}