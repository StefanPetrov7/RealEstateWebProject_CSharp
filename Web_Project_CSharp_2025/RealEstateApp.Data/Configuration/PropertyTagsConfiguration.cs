using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateApp.Data.Models;


namespace RealEstateApp.Data.Configuration
{
    public class PropertyTagsConfiguration : IEntityTypeConfiguration<PropertyTag>
    {
        // Fluent API for mapping table PropertyTag >> Many to Many relation
        public void Configure(EntityTypeBuilder<PropertyTag> builder)
        {
            builder.HasKey(x => new { x.PropertyId, x.TagId });

            builder.HasOne(x => x.Property)
                .WithMany(x => x.PropertyTags)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Tag)
             .WithMany(x => x.TagProperties)
             .IsRequired(true)
             .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
