using RealEstateApp.Data.DataServices.Contracts;
using System.Text.Json;

using RealEstateApp.Data.ImportModels;

namespace RealEstateApp.Data.DataServices
{
    public class SeedService
    {
        // string propertyHouseString = "D:\\Git\\RealEstateWebProject_CSharp\\Web_Project_CSharp_2025\\RealEstateApp.Data\\JsonImportData\\imot.bg-houses-Sofia-raw-data-2021-03-18.json";
        string propertyAppartementsString = "D:\\Git\\RealEstateWebProject_CSharp\\Web_Project_CSharp_2025\\RealEstateApp.Data\\JsonImportData\\imot.bg-raw-data-2021-03-18.json";

        private readonly IPropertyService service;
        public SeedService(IPropertyService service)
        {
            this.service = service;
        }

        public void RunSeed()
        {
            if (this.service.HasPropertyBeenAdded())
            {
                return;
            }
            //ImportProperties(propertyHouseString);
            ImportProperties(propertyAppartementsString);
        }


        public void ImportProperties(string fileLocation)
        {
            if (File.Exists(fileLocation) == false)
            {
                throw new FileNotFoundException("Json path to folder is not correct.");
            }
            var jsonProperties = JsonSerializer.Deserialize<IEnumerable<JsonImportModel>>(File.ReadAllText(fileLocation))!;

            foreach (var prop in jsonProperties)
            {
                this.service.AddProperty(prop.District, prop.Floor, prop.TotalFloor, prop.Size, prop.YardSize, prop.Year, prop.Type, prop.BuildingType, prop.Price);
                Console.Write(".");
            }
        }
    }
}
