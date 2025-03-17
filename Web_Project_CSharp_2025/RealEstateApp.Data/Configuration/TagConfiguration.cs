using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Data.Configuration
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.HasData(SeedTags());
        }

        private IEnumerable<Tag> SeedTags()
        {
            ICollection<Tag> tags = new HashSet<Tag>()
            {
                new Tag()
                {
                    Id = Guid.Parse("9b0b4ad3-4069-45a4-b5e6-74ae05ba9295"),    
                    Name = "Expensive"
                },
                new Tag()
                {
                    Id = Guid.Parse("afb72774-282e-4c0a-9f5d-5b604776e124"),
                    Name = "Constructed"
                },  
                new Tag()
                {
                    Id = Guid.Parse("f4402520-89c6-4dcc-a2dc-2e4d25572188"),
                    Name = "Size"
                }, 
                new Tag()
                {
                    Id = Guid.Parse("25a30cf6-43fe-4cbe-9f35-f71d40eba0d3"),    
                    Name = "Floor"
                },
            };

            return tags;

        }
    }

}
