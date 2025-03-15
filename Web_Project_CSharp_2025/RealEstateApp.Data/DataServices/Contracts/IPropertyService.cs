using RealEstateApp.Data.ImportModels;
using RealEstateApp.Data.Models;

namespace RealEstateApp.Data.DataServices.Contracts
{
    public interface IPropertyService 
    {
        void AddProperty(string district, int floor, int totalFloor, int size, int yardSize, int year, string propertyType, string buildingType, int price);

        bool HasPropertyBeenAdded();
    }
}
