using Microsoft.EntityFrameworkCore;
using RealtorMVC.Abstraction.FluentEmail;
using RealtorMVC.BLL.Interfaces;
using RealtorMVC.BLL.Service.Auth;
using RealtorMVC.DAL.EF;
using RealtorMVC.DAL.Repositories.Interfaces;
using RealtorMVC.DAL.Repositories;
using RealtorMVC.FluentEmail.Services;
using Microsoft.AspNetCore.Identity;
using RealtorMVC.DAL.Entities;
using RealtorMVC.Extentions;
using RealtorMVC.Common.Extensions;
using RealtorMVC.Common.Configs;
using RealtorMVC.BLL.Profiles;
using RealtorMVC.Seeding.Extentions;
using Microsoft.Data.SqlClient;
using System.Web.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RealtorMVC.BLL.Service;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Any;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Configs
builder.Services.ConfigsAssembly(builder.Configuration, opt => opt
       .AddConfig<JwtConfig>()
       .AddConfig<EmailConfig>()
       .AddConfig<UserAvatarConfig>()
       .AddConfig<ApartmentImagesConfig>()
       .AddConfig<AuthConfig>());

var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(key: Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtConfig:Secret")!)),
    ValidateIssuer = false,
    ValidateAudience = false,
    RequireExpirationTime = false,
    ValidateLifetime = true
};
builder.Services.AddAuthentication(configureOptions: x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(x =>
    {
        x.SaveToken = true;
        x.TokenValidationParameters = tokenValidationParameters;
    });

builder.Services.AddSingleton(tokenValidationParameters);

builder.Services.AddAutoMapper(typeof(AuthProfile));

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailConfirmationService, EmailConfirmationService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IApartmentService, ApartmentService>();

// Identity
builder.Services.AddIdentity<User, IdentityRole<Guid>>()
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider);

// Fluent Email
builder.Services.FluentEmail(builder.Configuration);

// Seeding
builder.Services.AddSeeding();

// Add services to the container.
builder.Services.AddControllersWithViews();

// CORS
builder.Services.AddCors(options => options
    .AddDefaultPolicy(build => build
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()));

var app = builder.Build();

await app.ApplySeedingAsync();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Uploads")),
    RequestPath = "/Uploads"
});

app.UseHttpsRedirection();
app.UseCors(
    opt => opt.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}/{id}",
    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });

app.Run();
