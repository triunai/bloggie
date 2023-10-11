using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace CodePulse.API.Controllers
{
    // https://localhost:7192/api/categories
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;


        // Constructor
        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        
        // POST method to create a category
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody]CreateCategoryRequestDto request)
        {
            // creating a new domain model with dto values

            var category = new Categories
            {
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };

            //this method saves to database and returns the passed category
            await categoryRepository.CreateCategoryAsync(category);

            // Creating a response dto from saved domain model

            var response = new CategoryDto
            {
                Id = category.Id,
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };


            return Ok(response);
        }


        // GET method (action) to get all category
        // /api/categories
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            // fetching all categories from database using our domain model (remember how api communicates with db using domain model)
            var categories = await categoryRepository.GetAllCategoriesAsync();

            // instantizing an empty list to hold the response dtos
            var response = new List<CategoryDto>();


            // use a loop here to go through all the get data
            foreach (var category in categories)
            {
                // converting the "categories"
                response.Add(new CategoryDto {
                    Id = category.Id,
                    Name = category.Name,
                    UrlHandle = category.UrlHandle
                });
            }

            return Ok(response);
        }




        // http://localhost:7179/api/categories/{Guid} , https://localhost:7100/api/Categories/d9641376-1e00-4a05-b2bd-08dbc6fca003
        // shorthand for FromRoute, no need to add it in the param then
        [HttpGet("{Id:guid}")]
        public async Task<IActionResult> GetCategory(Guid Id)
        {
            var existingCategory = await categoryRepository.GetSingleCategory(Id);

            if(existingCategory is null)
            {
                return NotFound("its literally null"); // 404
            }

            var response = new CategoryDto
            {
                Id = existingCategory.Id,
                Name = existingCategory.Name,
                UrlHandle = existingCategory.UrlHandle
            };
           

            return Ok(response); // 200 + response
        }

        // Put  method  http://localhost:7179/api/categories/{Guid}
        [HttpPut("{Id:guid}")]
        public async Task<IActionResult> UpdateCategory(Guid Id, UpdateCategoryRequestDto request)
        {
            // convert DTO to domain model type, convert this later into DTO type to abstract
            var newCategory = new Categories
            {
                Id = Id,
                Name = request.Name,
                UrlHandle = request.UrlHandle,

            };

            await categoryRepository.UpdateAsync(newCategory);


            if (newCategory is null)
            {
                return NotFound("Repository returned null, data doesnt exist in db then");
            }

            // form response based on dto
            var response = new CategoryDto
            {
                Id = newCategory.Id,
                Name = newCategory.Name,
                UrlHandle = newCategory.UrlHandle
            };

            return Ok(response);
        }


        //delete method :  http://localhost:7179/api/categories/{id}
        [HttpDelete("{Id:guid}")]
        public async Task<IActionResult> DeleteCategory(Guid Id)
        {
            var category = await categoryRepository.DeleteAsync(Id);
            if (category is null)
            {
                return NotFound("Data cant be deleted because it wasnt found");
            }

            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle,
            };

            return Ok(response);
        }

    }
}
