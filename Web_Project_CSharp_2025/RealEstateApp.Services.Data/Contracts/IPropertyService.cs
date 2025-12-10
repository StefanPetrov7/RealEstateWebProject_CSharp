

using RealEstateApp.Web.ViewModels.Admin.Reports;
using RealEstateApp.Web.ViewModels.Property;

namespace RealEstateApp.Data.DataServices.Contracts
{
    public interface IPropertyService
    {
        Task AddPropertyAsync(string district, byte? floor, byte? totalFloor, int size, int? yardSize, int? year, string propertyType, string buildingType, int? price, string imageUrl = null);

        Task<PropertyViewModel> GetPropertyDetailsByIdAsync(Guid id);

        Task<bool> HasPropertyBeenAdded();

        Task<IEnumerable<PropertyViewModel>> IndexGetAllPropertiesAsync();

        Task<IEnumerable<PropertyViewModel>> IndexGetAllActivePropertiesAsync();

        Task<bool> UpdatePropertyAsync(PropertyViewModel property);

        Task<bool> SoftDeletePropertyAsync(Guid id);

        Task<bool> RestorePropertyAsync(Guid id);

        Task<PropertyReportViewModel> GetPropertyStatusReportAsync();

        Task<IEnumerable<PropertyTrendReportViewModel>> GetPropertyTrendReportAsync();

    }
}
