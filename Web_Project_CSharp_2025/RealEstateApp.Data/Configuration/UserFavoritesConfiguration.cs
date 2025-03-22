using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RealEstateApp.Data.Models;


namespace RealEstateApp.Data.Configuration
{
    public class UserFavoritesConfiguration : IEntityTypeConfiguration<UserFavorites>
    {
        public void Configure(EntityTypeBuilder<UserFavorites> builder)
        {
            builder.HasKey(x => new { x.ApplicationUserId, x.FavoriteId });

            builder.HasOne(x => x.User)
                .WithMany(x => x.Favorites)
                .HasForeignKey(x => x.ApplicationUserId);


            builder.HasOne(x => x.Favorites)
           .WithMany(x => x.Favorites)
           .HasForeignKey(x => x.FavoriteId);


        }
    }
}
