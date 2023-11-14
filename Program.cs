using CodePulse.API.Data;
using CodePulse.API.Repositories.Implementation;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CodePulseConnectionString")); //passing the string 
});



builder.Services.AddDbContext<AuthDbContext>(options =>
{
  options.UseSqlServer(builder.Configuration.GetConnectionString("CodePulseConnectionString")); //you can provide a different connection strig in case you want to use another auth db
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// injecting service program cs ensure its before the app, check its pattern implementation if youre unsure, usually placed before the app.build
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();

builder.Services.AddIdentityCore<IdentityUser>()
  .AddRoles<IdentityRole>()
  .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("CodePulse")
  .AddEntityFrameworkStores<AuthDbContext>()
  .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{

  //// Password settings
  //options.Password.RequireDigit = true;
  //options.Password.RequiredLength = 8;
  //options.Password.RequireNonAlphanumeric = false;
  //options.Password.RequireUppercase = true;
  //options.Password.RequireLowercase = false;

  //// Lockout settings
  //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
  //options.Lockout.MaxFailedAccessAttempts = 5;

  //// User settings
  //options.User.RequireUniqueEmail = true;

  options.Password.RequireDigit = false;
  options.Password.RequireLowercase = false;
  options.Password.RequireNonAlphanumeric = false;
  options.Password.RequireUppercase = false;
  options.Password.RequiredLength = 6;
  options.Password.RequiredUniqueChars = 1;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(options =>
  {
    options.TokenValidationParameters = new TokenValidationParameters // define how tokens should ve validated
    {
      AuthenticationType = "Jwt", //type of authentication
      ValidateIssuer = true, // ensures token issuer is trusted
      ValidateAudience = true, // ensures token is intended for that application 
      ValidateLifetime = true, // checks if token hasnt expired
      ValidateIssuerSigningKey = true, // confirms token was signed with a key that the app recognises
      ValidIssuer = builder.Configuration["Jwt:Issuer"], //ensure spellling matches here with appsettings.json
      ValidAudience = builder.Configuration["Jwt:Audience"],
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))  // not using RSA, because symmetric is faster
    };
  });

var app = builder.Build();


// START OF HTTP PIPELINE!

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Configuring CORS to allow angular to speak to asp.net
app.UseCors(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
);

app.UseAuthentication(); // <--- Added this for jwt 
app.UseAuthorization();

app.UseStaticFiles(new StaticFileOptions
{
  // Point to the physical "Images" folder in the application directory
  FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
  RequestPath = "/Images"
});

app.MapControllers();

app.Run();
