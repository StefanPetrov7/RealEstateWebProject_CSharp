using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Common;
using RealEstateApp.Data.DataServices.Contracts;
using RealEstateApp.Data.ImportModels;
using RealEstateApp.Data.Models;
using System;
using System.Text.Json;


namespace RealEstateApp.Data.DataServices
{
    public class SeedService : ISeedService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserStore<ApplicationUser> userStore;
        private readonly RoleManager<IdentityRole<Guid>> roleManager;
        private readonly IPropertyService propertyService;
        private readonly IConfiguration configuration;

        private readonly string propertyAppartementsString = "D:\\Git\\RealEstateWebProject_CSharp\\Web_Project_CSharp_2025\\RealEstateApp.Data\\JsonImportData\\imot.bg-raw-data-2021-03-18.json";
        private readonly string[] DefaultRoles = new string[] { AppConstants.AdminRoleName, AppConstants.UserRoleName, AppConstants.GoldUserRoleName, AppConstants.PlatinumUserRoleName };

        public SeedService(
            IPropertyService propService,
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            RoleManager<IdentityRole<Guid>> roleManager,
            IConfiguration configuration
            )
        {
            this.propertyService = propService;
            this.userManager = userManager;
            this.userStore = userStore;
            this.roleManager = roleManager;
            this.configuration = configuration;   
        }

        public async Task SeedDefaultProperties()
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

        public async Task SeedIdentityAsync()
        {
            await this.SeedRolesAsync();
            await this.SeedUSersAsync();
        }

        private async Task SeedRolesAsync()
        {
            foreach (string defaultRole in DefaultRoles)
            {
                bool roleExists = await this.roleManager.RoleExistsAsync(defaultRole);

                if (roleExists == false)
                {
                    IdentityRole<Guid> identityRole = new IdentityRole<Guid>
                    {
                        Id = Guid.NewGuid(),
                        Name = defaultRole,
                        NormalizedName = defaultRole.ToUpper()
                    };

                    IdentityResult result = await roleManager.CreateAsync(identityRole);

                    if (result.Succeeded == false)
                    {
                        throw new Exception($"Exception while creating new identity role: {defaultRole}!");
                    }
                }
            }
        }

        private async Task SeedUSersAsync()
        {
            string? adminMail = this.configuration["UserSeed:TestAdmin:Email"];
            string? adminPassword = this.configuration["UserSeed:TestAdmin:Password"];

            ApplicationUser? userSeed = await userManager.FindByNameAsync(adminMail);

            if (userSeed == null)
            {
                ApplicationUser adminUser = new ApplicationUser()
                {
                    UserName = adminMail,
                    Email = adminMail,
                    EmailConfirmed = true,
                };

                IdentityResult result = await this.userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded == false)
                {
                    throw new Exception($"Exception while creating admin user {adminMail}!");
                }

                result = await this.userManager.AddToRoleAsync(adminUser, AppConstants.AdminRoleName);

                if (result.Succeeded == false)
                {
                    throw new Exception($"Exception while adding {AppConstants.AdminRoleName} role to the {adminMail} user");
                }
            }
            else if (userSeed.IsDeleted == true) 
            {
                userSeed.IsDeleted = false; 
                await userManager.UpdateAsync(userSeed);

                if (!await userManager.IsInRoleAsync(userSeed, AppConstants.AdminRoleName))
                {
                    await userManager.AddToRoleAsync(userSeed, AppConstants.AdminRoleName);
                }
            }
        }
    }
}
