using Azure.Core;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Implementation;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BlogPostController : ControllerBase
  {


    //private readonly fields
    private readonly IBlogPostRepository blogpostRepository;
    private readonly ICategoryRepository categoryRepository;

    public BlogPostController(
      IBlogPostRepository blogpostRepository,
      ICategoryRepository categoryRepository
      ) // <-- inject contract here to use repository pattern
    {
      this.blogpostRepository = blogpostRepository;
      this.categoryRepository = categoryRepository;
    }
    // POST: [apiBaseUrl]/api/blogposts
    [HttpPost]
    public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDto request)
    {
      // Inline Validation (Optional but recommended)
      if (string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.ShortDescription))
      {
        return BadRequest("Title and Short Description are required.");
      }
      // be elegant, be disciplined
      if (request == null)
      {
        return BadRequest("Invalid blog post data");
      }

      try
      {
        //convert data from DTO type to domain model to process/transform it
        var blogPost = new BlogPost
        {
          Title = request.Title,
          ShortDescription = request.ShortDescription,
          Content = request.Content,
          FeaturedImageUrl = request.FeaturedImageUrl,
          UrlHandle = request.UrlHandle,
          PublishedDate = request.PublishedDate,
          Author = request.Author,
          IsVisible = request.IsVisible,
          Categories = new List<Categories>(),
        };

        // write loop to iterate through cateogories variable
        foreach (var categoryId in request.Categories)
        {
          var existingCategory = await categoryRepository.GetCategoryById(categoryId);
          if (existingCategory != null)
          {
            blogPost.Categories.Add(existingCategory);
          };
        }



        // use repository to create, dont expose db context here
        // automatically adds and saves it
        await blogpostRepository.CreateBlogPostAsync(blogPost);


        // just for elegant handling and best practice
        if (blogPost != null)
        {
          //transform dto back to domain model
          var abstractedResponse = new BlogPostDto
          {
            Id = blogPost.Id,
            Title = request.Title,
            ShortDescription = request.ShortDescription,
            Content = request.Content,
            FeaturedImageUrl = request.FeaturedImageUrl,
            UrlHandle = request.UrlHandle,
            PublishedDate = request.PublishedDate,
            Author = request.Author,
            IsVisible = request.IsVisible,

            Categories = blogPost.Categories.Select(x => new CategoryDto
            {
              Id = x.Id,
              Name = x.Name,
              UrlHandle = x.UrlHandle,
            }).ToList(),
          };

          return Ok(abstractedResponse); //200 success
        }
      }
      catch (Exception ex)
      {
        return StatusCode(500, $"An error occurred: {ex.Message}");
      }
      return Unauthorized(request);
    }


    // GET: [apiBaseUrl]/api/blogposts
    [HttpGet]
    public async Task<IActionResult> GetAllBlogPosts()
    {
      // fetch categories first using repo method
      var blogposts = await blogpostRepository.GetAllBlogpostsAsync();




      // instantize an empty list to hold the response data from ur async method
      var response = new List<BlogPostDto>();

      // use a loop  here to go thhrough all the get data
      foreach (var blogPost in blogposts)
      {
        response.Add(new BlogPostDto
        {
          Id = blogPost.Id,
          Title = blogPost.Title,
          ShortDescription = blogPost.ShortDescription,
          Content = blogPost.Content,
          FeaturedImageUrl = blogPost.FeaturedImageUrl,
          UrlHandle = blogPost.UrlHandle,
          PublishedDate = blogPost.PublishedDate,
          Author = blogPost.Author,
          IsVisible = blogPost.IsVisible,

          Categories = blogPost.Categories.Select(x => new CategoryDto
          {
            Id = x.Id,
            Name = x.Name,
            UrlHandle = x.UrlHandle,
          }).ToList(),

        });
      }

      //return the responsei 
      return Ok(response);
    }

    //[HttpGet("{Id:guid}")]
    // GET : {apiBaseUrl}/api/blogposts/{id}

    [HttpGet("{Id:guid}")] // removed space could be error?
    public async Task <IActionResult> GetBlogpost(Guid Id)
    {
      var existingBlogPost = await blogpostRepository.GetBlogPostById(Id);

      if(existingBlogPost is null)
      {
        return NotFound("This id couldnt be found in blogpost tables");
      };
      // initialize dto object to store data
      var response = new BlogPostDto
      {
        Id = existingBlogPost.Id,
        Title = existingBlogPost.Title,
        ShortDescription = existingBlogPost.ShortDescription,
        Content = existingBlogPost.Content,
        FeaturedImageUrl = existingBlogPost.FeaturedImageUrl,
        UrlHandle = existingBlogPost.UrlHandle,
        PublishedDate = existingBlogPost.PublishedDate,
        Author = existingBlogPost.Author,
        IsVisible = existingBlogPost.IsVisible,

        Categories = existingBlogPost.Categories.Select(x => new CategoryDto
        {
          Id = x.Id,
          Name = x.Name,
          UrlHandle = x.UrlHandle,
        }).ToList(),
      };

      return Ok(response);
      }
    }
  }
