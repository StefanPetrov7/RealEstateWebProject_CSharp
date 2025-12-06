using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using RealEstateApp.Data;
using RealEstateApp.Data.Repository;
using RealEstateApp.Data.Repository.Contracts;
using RealEstateApp.Data.DataServices;
using RealEstateApp.Data.DataServices.Contracts;
using RealEstateApp.Data.Models;
using RealEstateApp.Web.Infrastructure;
using RealEstateApp.Web.Infrastructure.Extensions;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Web.Areas.Identity.Infrastructure;

namespace RealEstateApp.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                                    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(cfg =>
            {
                cfg.SignIn.RequireConfirmedAccount = true;
                ConfigureIdentity(builder, cfg);
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddRoles<IdentityRole<Guid>>()
                .AddUserManager<UserManager<ApplicationUser>>()
                .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(cfg =>
            {
                cfg.LoginPath = "/Identity/Account/Login";
            });

            //builder.Services.AddScoped<IRepository<Property, Guid>, BaseRepository<Property, Guid>>();
            //builder.Services.AddScoped<IRepository<District, Guid>, BaseRepository<District, Guid>>();
            //builder.Services.AddScoped<IRepository<PropertyType, Guid>, BaseRepository<PropertyType, Guid>>();
            //builder.Services.AddScoped<IRepository<BuildingType, Guid>, BaseRepository<BuildingType, Guid>>();
            //builder.Services.AddScoped<IRepository<Favorite, Guid>, BaseRepository<Favorite, Guid>>();
            //builder.Services.AddScoped<IRepository<PropertyFavorite, object>, BaseRepository<PropertyFavorite, object>>();
            // My extension method
            // Below extension method register all the above repositories!
            builder.Services.RegisterRepositories(typeof(ApplicationUser).Assembly);

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            //builder.Services.AddScoped<ISeedService, SeedService>();
            //builder.Services.AddScoped<IPropertyService, PropertyService>();
            //builder.Services.AddScoped<IFavoriteService, FavoriteService>();
            //builder.Services.AddScoped<IValidationService, ValidationService>();
            // My extension method
            // Below extension method register all the above services!

            builder.Services.RegisterUserServices(typeof(FavoriteService).Assembly);

            // Below service is adding my custom sign in manager to avoid log in on users which have been deleted by the admin. 
            builder.Services.AddScoped<SignInManager<ApplicationUser>, CustomSignInManager>();


            // Attribute added to the Web App Controllers only, not to the Web API Controllers 
            // Used for the [AutoValidateAntiforgeryToken] attribute. 
            builder.Services.AddControllersWithViews();

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode={0}");

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            // My extension methods >> seeding roles and one test admin user and seeding default properties
            app.SeedDefaultIdentity();
            app.SeedDefaultProperties();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "area",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }

        private static void ConfigureIdentity(WebApplicationBuilder builder, IdentityOptions cfg)
        {
            cfg.Password.RequireDigit = builder.Configuration.GetValue<bool>("Identity:Password:RequireDigits");
            cfg.Password.RequireLowercase = builder.Configuration.GetValue<bool>("Identity:Password:RequireLowercase");
            cfg.Password.RequireUppercase = builder.Configuration.GetValue<bool>("Identity:Password:RequireUppercase");
            cfg.Password.RequireNonAlphanumeric = builder.Configuration.GetValue<bool>("Identity:Password:RequireNonAlphanumerical");
            cfg.Password.RequiredLength = builder.Configuration.GetValue<int>("Identity:Password:RequiredLength");
            cfg.Password.RequiredUniqueChars = builder.Configuration.GetValue<int>("Identity:Password:RequiredUniqueCharacters");

            cfg.SignIn.RequireConfirmedAccount = builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedAccount");
            cfg.SignIn.RequireConfirmedEmail = builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedEmail");
            cfg.SignIn.RequireConfirmedPhoneNumber = builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedPhoneNumber");

            cfg.User.RequireUniqueEmail = builder.Configuration.GetValue<bool>("Identity:User:RequireUniqueEmail");
        }
    }
}
