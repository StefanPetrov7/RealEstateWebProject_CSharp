using RealEstateApp.Data.DataServices.Contracts;
using System.Text.Json;

using RealEstateApp.Data.ImportModels;
using System.Runtime.CompilerServices;

namespace RealEstateApp.Data.DataServices
{
    public class SeedService : ISeedService
    {
        string propertyAppartementsString = "D:\\Git\\RealEstateWebProject_CSharp\\Web_Project_CSharp_2025\\RealEstateApp.Data\\JsonImportData\\imot.bg-raw-data-2021-03-18.json";

        private readonly IPropertyService service;
        public SeedService(IPropertyService service)
        {
            this.service = service;
        }

        public async Task RunSeed()
        {
            if (await this.service.HasPropertyBeenAdded())
            {
                return;
            }

           await ImportProperties(propertyAppartementsString);
        }


        public async Task  ImportProperties(string fileLocation)
        {
            if (File.Exists(fileLocation) == false)
            {
                throw new FileNotFoundException("Json path to folder is not correct.");
            }
            var jsonProperties = JsonSerializer.Deserialize<IEnumerable<JsonImportModel>>(File.ReadAllText(fileLocation))!;

            foreach (var prop in jsonProperties)
            {
                await this.service.AddProperty(prop.District, prop.Floor, prop.TotalFloor, prop.Size, prop.YardSize, prop.Year, prop.Type, prop.BuildingType, prop.Price);
                Console.Write(".");
            }
        }
    }
}
