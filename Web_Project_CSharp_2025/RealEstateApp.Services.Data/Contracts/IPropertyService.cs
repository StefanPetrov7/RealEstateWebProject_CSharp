﻿

using RealEstateApp.Web.ViewModels.Property;

namespace RealEstateApp.Data.DataServices.Contracts
{
    public interface IPropertyService 
    {
        Task AddProperty(string district, int floor, int totalFloor, int size, int? yardSize, int? year, string propertyType, string buildingType, int? price, string imageUrl = null);

        Task<PropertyViewModel> GetPropertyDetailsByIdAsync(Guid id);

        Task<bool> HasPropertyBeenAdded();

        Task<IEnumerable<PropertyViewModel>> IndexGetAllPropertiesAsync();
    }
}
