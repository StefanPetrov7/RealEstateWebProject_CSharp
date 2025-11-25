using RealEstateApp.Data.ImportModels;
using RealEstateApp.Common;

using RealEstateApp.Data.DataServices.Contracts;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;


namespace RealEstateApp.Data.DataServices
{
    public class SeedService : ISeedService
    {
        private readonly string propertyAppartementsString = "D:\\Git\\RealEstateWebProject_CSharp\\Web_Project_CSharp_2025\\RealEstateApp.Data\\JsonImportData\\imot.bg-raw-data-2021-03-18.json";
        private readonly string[] DefaultRoles = new string[] { AppConstants.AdminRoleName, AppConstants.UserRoleName };

        private readonly IPropertyService propertyService;
        public SeedService(IPropertyService service)
        {
            this.propertyService = service;
        }

        public async Task RunSeed()
        {
            if (await this.propertyService.HasPropertyBeenAdded())
            {
                return;
            }

            await ImportProperties(propertyAppartementsString);
        }


        public async Task ImportProperties(string fileLocation)
        {
            if (File.Exists(fileLocation) == false)
            {
                throw new FileNotFoundException("Json path to folder is not correct.");
            }

            var jsonProperties = JsonSerializer.Deserialize<IEnumerable<JsonImportModel>>(File.ReadAllText(fileLocation))!;

            foreach (var prop in jsonProperties)
            {
                await this.propertyService.AddPropertyAsync(prop.District, prop.Floor, prop.TotalFloor, prop.Size, prop.YardSize, prop.Year, prop.Type, prop.BuildingType, prop.Price);
                Console.Write(".");
            }
        }

        public async void SeedIdentity(IServiceProvider serviceProvider)
        {
            await SeedRolesAsync(serviceProvider);
        }

        private async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (string defaultRole in DefaultRoles)
            {
                bool roleExists = await roleManager.RoleExistsAsync(defaultRole);

                if (roleExists == false)
                {
                    IdentityRole identityRole = new IdentityRole(defaultRole);
                    IdentityResult result = await roleManager.CreateAsync(identityRole);

                    if (result.Succeeded == false)
                    {
                        throw new Exception($"Exception while creating new identity role: {defaultRole}!");
                    }
                }
            }
        }
    }
}
