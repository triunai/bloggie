using CodePulse.API.Models.Domain;
namespace CodePulse.API.Repositories.Interface
{
    public interface ICategoryRepository
    {
        //can only define definitions not implementations here

        Task<Categories> CreateCategoryAsync(Categories category);

        //using IEnumerable here to ensure data is read only, suited for get methods, and also has Deferred Execution
        Task<IEnumerable<Categories>> GetAllCategoriesAsync();

        // no need to use IEnumerable because we are dealing with a single response
        // will return a category or return null
        Task<Categories?> GetCategoryById(Guid Id);

        // will return an updated object or null
        Task<Categories?> UpdateAsync(Categories newCategory);

        Task <Categories?> DeleteAsync(Guid Id);
    }
}


