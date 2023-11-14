using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data
{
  public class AuthDbContext: IdentityDbContext // uses the nuget package we just installed in dependencies
  {
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
      // reader role only to let people read, ONLY GET operatiosn
      // writer is for admin , post get etc
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      // create reader and writer role
      var readerRoleId = "51a65cfe-5ed3-46da-8103-44bbc580843a"; // <--- check this in gpt if it should be constant
      var writerRoleId = "d2f3434d-319c-41df-8223-7c594f064594"; // <--- check this in gpt if it should be constant

      // define roles , reader and writer
      var roles = new List<IdentityRole>
      {
        new IdentityRole()
        {
          Id = readerRoleId,
          Name = "Reader",
          NormalizedName = "Reader".ToUpper(),
          ConcurrencyStamp = readerRoleId
        },
        new IdentityRole()
        {
          Id = writerRoleId,
          Name = "Writer",
          NormalizedName = "Writer".ToUpper(),
          ConcurrencyStamp = writerRoleId
        }
      };

      // Seed the roles
      builder.Entity<IdentityRole>().HasData(roles);

      // create an admin user
      var adminUserId = "50d933cc-afbf-4f5b-9946-939965e27880";
      var admin = new IdentityUser() 
      {
        Id = adminUserId,
        UserName = "admin@codepulse.com",
        Email = "admin@codepulse.com",
        NormalizedEmail = "admin@codepulse.com".ToUpper(),
        NormalizedUserName = "admin@codepulse.com".ToUpper(),
      };
      admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");
      builder.Entity<IdentityUser>().HasData(admin);

      // giving roles to admin

      var adminRoles = new List<IdentityUserRole<string>>()
      {
        // for readers (public)
        new()
        {
          UserId = adminUserId,
          RoleId = readerRoleId,
        },
        // for writers (admin)
        new()
        {
          UserId = adminUserId,
          RoleId = writerRoleId,
        }
      };
      builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
    }
    

  }
}
