using Microsoft.EntityFrameworkCore;
using RealEstateApp.Data.DataServices.Contracts;
using RealEstateApp.Data.Models;

using static RealEstateApp.Common.AppConstants;

namespace RealEstateApp.Data.DataServices
{
    public class PropertyService : IPropertyService
    {
        private readonly ApplicationDbContext dbContext;
        public PropertyService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> HasPropertyBeenAdded()
        {
            return await this.dbContext.Properties.AnyAsync(); ;
        }

        public async Task AddProperty(string district, int floor, int totalFloor, int size, int? yardSize, int? year, string propertyType, string buildingType, int? price, string imageUrl = null)
        {

            var property = new Property
            {
                // >> Property ctor is initializing the Id by default. 
                // Id = Guid.NewGuid(), 
                Size = size,
                Price = price <= 0 ? null : price,
                Floor = floor <= 0 || floor >= 255 ? null : (byte)floor,
                TotalFloors = totalFloor <= 0 || totalFloor >= 255 ? null : (byte)totalFloor,
                YardSize = yardSize <= 0 ? null : yardSize,
                Year = year <= 1800 ? null : year,
                DateAdded = DateTime.Now,
                ImageUrl = imageUrl ?? PropertyDefaultImageUrl,
            };

            var dbDistrict = await dbContext.Districts.FirstOrDefaultAsync(x => x.Name == district);

            if (dbDistrict == null)
            {
                dbDistrict = new District { Name = district };
            }

            property.District = dbDistrict;

            var dbPropertyType = await dbContext.PropertyTypes.FirstOrDefaultAsync(x => x.Name == propertyType);

            if (dbPropertyType == null)
            {
                dbPropertyType = new PropertyType { Name = propertyType };
            }

            property.PropertyType = dbPropertyType;

            var dbBuildingType = await dbContext.BuildingTypes.FirstOrDefaultAsync(x => x.Name == buildingType);

            if (dbBuildingType == null)
            {
                dbBuildingType = new BuildingType { Name = buildingType };
            }

            property.BuildingType = dbBuildingType;

            using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                await dbContext.AddAsync(property);
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new InvalidOperationException($"DB Add Property Transaction failed:  {ex.Message}");
            }
        }
    }
}
