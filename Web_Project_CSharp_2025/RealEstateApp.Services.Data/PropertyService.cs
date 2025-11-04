using Microsoft.EntityFrameworkCore;
using RealEstateApp.Data.DataServices.Contracts;
using RealEstateApp.Data.Models;
using RealEstateApp.Data.Repository.Contracts;
using RealEstateApp.Web.ViewModels.Property;
using static RealEstateApp.Common.AppConstants;

namespace RealEstateApp.Data.DataServices
{
    public class PropertyService : IPropertyService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IRepository<Property, Guid> propertyRepository;

        public PropertyService(ApplicationDbContext dbContext, IRepository<Property, Guid> repository)
        {
            this.dbContext = dbContext;
            this.propertyRepository = repository;
        }

        public async Task<bool> HasPropertyBeenAdded()
        {
            return await this.dbContext.Properties.AnyAsync(); ;
        }

        public async Task AddPropertyAsync(string district, int floor, int totalFloor, int size, int? yardSize, int? year, string propertyType, string buildingType, int? price, string imageUrl = null)
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

        public async Task<PropertyViewModel> GetPropertyDetailsByIdAsync(Guid Id)
        {
            PropertyViewModel? property = await this.propertyRepository.GetAllAttached()
                .Where(x => x.Id == Id)
                .Select(x => new PropertyViewModel
                {
                   Name = x.PropertyType!.Name,
                   BuildingType = x.BuildingType!.Name,
                   DistrictName = x.District!.Name,    
                   Floor = x.Floor,
                   TotalFloors = x.TotalFloors,
                   Price = x.Price,
                   Size = x.Size,
                   YardSize = x.YardSize,
                   Year = x.Year,
                   DateAdded = x.DateAdded,
                   ImageUrl = x.ImageUrl,
                })
             .FirstOrDefaultAsync();

            return property!;
        }

        public async Task<IEnumerable<PropertyViewModel>> IndexGetAllPropertiesAsync()
        {
            IEnumerable<PropertyViewModel> allProperties = await propertyRepository.GetAllAttached()
                   .Select(x => new PropertyViewModel
                   {
                       Id = x.Id.ToString(),
                       Name = x.PropertyType!.Name,
                       BuildingType = x.BuildingType!.Name,
                       DistrictName = x.District!.Name,
                       Floor = x.Floor,
                       Price = x.Price,
                       Size = x.Size,
                       Year = x.Year,
                       DateAdded = x.DateAdded,
                       ImageUrl = x.ImageUrl,
                   })
                    .ToArrayAsync();

            return allProperties;
        }
    }
}
