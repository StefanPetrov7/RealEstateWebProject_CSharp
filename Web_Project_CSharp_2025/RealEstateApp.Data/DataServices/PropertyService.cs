using RealEstateApp.Data;
using RealEstateApp.Data.DataServices.Contracts;
using RealEstateApp.Data.ImportModels;
using RealEstateApp.Data.Models;

namespace RealEstateApp.Data.DataServices
{
    public class PropertyService : IPropertyService
    {
        private readonly ApplicationDbContext dbContext;
        public PropertyService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool HasPropertyBeenAdded() 
        {
            return this.dbContext.Properties.Any(); 
        }

        public void AddProperty(string district, int floor, int totalFloor, int size, int yardSize, int year, string propertyType, string buildingType, int price)
        {

            var property = new Property
            {
                Id = Guid.NewGuid(),
                Size = size,
                Price = price <= 0 ? null : price,
                Floor = floor <= 0 || floor >= 255 ? null : (byte)floor,
                TotalFloors = totalFloor <= 0 || totalFloor >= 255 ? null : (byte)totalFloor,
                YardSize = yardSize <= 0 ? null : yardSize,
                Year = year <= 1800 ? null : year,
                DateAdded = DateTime.Now,
            };

            var dbDistrict = dbContext.Districts.FirstOrDefault(x => x.Name == district);

            if (dbDistrict == null)
            {
                dbDistrict = new District { Name = district };
            }

            property.District = dbDistrict;

            var dbPropertyType = dbContext.PropertyTypes.FirstOrDefault(x => x.Name == propertyType);

            if (dbPropertyType == null)
            {
                dbPropertyType = new PropertyType { Name = propertyType };
            }

            property.PropertyType = dbPropertyType;

            var dbBuildingType = dbContext.BuildingTypes.FirstOrDefault(x => x.Name == buildingType);

            if (dbBuildingType == null)
            {
                dbBuildingType = new BuildingType { Name = buildingType };
            }

            property.BuildingType = dbBuildingType;

            dbContext.Add(property);
            dbContext.SaveChanges();

        }
    }
}
