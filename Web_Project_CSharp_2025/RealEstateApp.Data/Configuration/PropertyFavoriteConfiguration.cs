using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateApp.Data.Models;

namespace RealEstateApp.Data.Configuration
{
    public class PropertyFavoriteConfiguration : IEntityTypeConfiguration<PropertyFavorite>
    {
        public void Configure(EntityTypeBuilder<PropertyFavorite> builder)
        {
            builder.HasKey(x => new { x.PropertyId, x.FavoriteId });

            builder.Property(x => x.IsDeleted)
                .HasDefaultValue(false);

            builder.HasOne(x => x.Property)
                .WithMany(x => x.PropertyFavorites)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Favorite)
             .WithMany(x => x.FavoriteProperties)
             .IsRequired(true)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
