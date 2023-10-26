using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Categories> CreateCategoryAsync(Categories category)
        {
            await dbContext.Categories.AddAsync(category); //method to add into database
            await dbContext.SaveChangesAsync(); //saving changes in databsae
            return category;
        }

        public async Task<IEnumerable<Categories>> GetAllCategoriesAsync()
        {
            return await dbContext.Categories.ToListAsync();
        }

        public async Task<Categories?> GetCategoryById(Guid Id) // <------ Added nullable operator to ensure return is safe
        {
            return await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<Categories?> UpdateAsync(Categories newCategory)
        {
           var matchingCategory = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == newCategory.Id);

            if (matchingCategory != null)
            {
                // ALTERNATE IMPLEMENTATION
                //matchingCategory.Name = category.Name;
                //matchingCategory.UrlHandle = category.UrlHandle

                dbContext.Entry(matchingCategory).CurrentValues.SetValues(newCategory);
                await dbContext.SaveChangesAsync();
                return matchingCategory;
            }
                return null;          
        }

        public async Task<Categories?> DeleteAsync(Guid Id)
        {

          var existingCategory = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == Id);
           
           if (existingCategory != null) { 
                dbContext.Categories.Remove(existingCategory);
                await dbContext.SaveChangesAsync();
                return existingCategory;
            }
            return null;
        }
    }
}
