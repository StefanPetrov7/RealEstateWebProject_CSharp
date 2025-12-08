using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateApp.Data.Models;

using static RealEstateApp.Common.AppConstants;
using static RealEstateApp.Common.EntityValidationConstants.Property;
using Property = RealEstateApp.Data.Models.Property;



namespace RealEstateApp.Data.Configuration
{
    public class PropertyConfiguration : IEntityTypeConfiguration<Property>
    {
        public void Configure(EntityTypeBuilder<Property> builder)
        {
            builder.Property(x => x.ImageUrl)
                .IsRequired(false)
                .HasMaxLength(PropertyImageUrlMaxLength)
                .HasDefaultValue(PropertyDefaultImageUrl);

            builder.HasQueryFilter(x => x.IsDeleted == false);
        }
    }
}
