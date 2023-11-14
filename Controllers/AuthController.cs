using CodePulse.API.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly UserManager<IdentityUser> userManager;

    public AuthController(UserManager<IdentityUser> userManager)
    {
      this.userManager = userManager;
    }

    // POST: {apiBaseUrl}/api/auth/login
    [HttpPost("login")]
    public async Task <IActionResult> Login([FromBody] LoginRequestDto request)
    {
      // checking the email
      var identityUser = await userManager.FindByEmailAsync(request.Email);

      if(identityUser is not null)
      {
        // check the password & confirming the password
        var checkPasswordResult = await userManager.CheckPasswordAsync(identityUser, request.Password);

        if(checkPasswordResult)
        {
          var roles = await userManager.GetRolesAsync(identityUser);
          // create a jwt token and response
          var response = new LoginResponseDto
          {
            Email = request.Email,
            Roles = roles.ToList(),
            Token = "CHANGETHISLATER"   // <--- Change this later!
          };
          return Ok();
        }
      }
      ModelState.AddModelError("", "Email or password is incorrect");
      return ValidationProblem(ModelState);
    }


    // POST: {apiBaseUrl}/api/auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
      // create IdentityUser object
      var user = new IdentityUser
      {
        UserName = request.Email?.Trim(),
        Email = request.Email?.Trim()
      };

      // creating our user here
      var identityResult = await userManager.CreateAsync(user, request.Password);

      if (identityResult.Succeeded)
      {
        // we only want to give Reader roles to these public users
        identityResult = await userManager.AddToRoleAsync(user, "Reader"); // <--- comes from AuthDbContext check over there to make sure

        if (identityResult.Succeeded)
        {
          return Ok();
        }
        // error handling block
        else
        {
          if (identityResult.Errors.Any())
          {
            foreach (var error in identityResult.Errors)
            {
              ModelState.AddModelError("", error.Description);
            }
          }
        }
      }
      // error handling block
      else
      {
        if(identityResult.Errors.Any())
        {
          foreach (var error in identityResult.Errors)
          {
            ModelState.AddModelError("", error.Description);
          }
        }
      }

      return ValidationProblem(ModelState);
    }

  }
}
