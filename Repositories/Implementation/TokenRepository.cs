using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CodePulse.API.Repositories.Implementation
{
  public class TokenRepository : ITokenRepository
  {
    private readonly IConfiguration configuration;

    public TokenRepository(IConfiguration configuration)
    {
      this.configuration = configuration;
    }
    public string CreateJwtToken(IdentityUser user, List<string> roles)
    {
      // create claimms from roles
      var claims = new List<Claim>
      {
        new Claim(ClaimTypes.Email, user.Email),
      };

      claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role))); // iterating through each role to convert it to a claim

      // use these claims to define JWT security token parameters
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
      var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // show viki
      var token = new JwtSecurityToken(
        issuer: configuration["Jwt:Issuer"],
        audience: configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.Now.AddMinutes(15),
        signingCredentials: credentials);

      // Return the token
      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}
