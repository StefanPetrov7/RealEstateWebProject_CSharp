using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Data.Models;
using System.Reflection;
using System.Reflection.Emit;

namespace RealEstateApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BuildingType> BuildingTypes { get; set; }

        public virtual DbSet<District> Districts { get; set; }

        public virtual DbSet<Property> Properties { get; set; }

        public virtual DbSet<PropertyFavorite> PropertyFavorites { get; set; }

        public virtual DbSet<PropertyType> PropertyTypes { get; set; }

        public virtual DbSet<Favorite> Favorites { get; set; }

        public virtual DbSet<UserFavorites> UserFavorites { get; set; }

        // Below configuration will be done in the Program.cs, TODO if needed Lazy Loading can be configured!

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder); 
        //}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
           
        }
    }
}