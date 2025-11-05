using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RealEstateApp.Data.Models;


namespace RealEstateApp.Data.Configuration
{
    public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
    {
        public void Configure(EntityTypeBuilder<Favorite> builder)
        {
            builder.HasOne(x => x.User)
                .WithMany(x => x.Favorites)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(x => x.IsDeleted == false);
        }
    }
}
