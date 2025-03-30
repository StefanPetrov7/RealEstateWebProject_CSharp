using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using RealEstateApp.Data;
using RealEstateApp.Data.DataServices.Contracts;
using RealEstateApp.Data.Models;
using RealEstateApp.Web.ViewModels.Property;
using RealEstateApp.Web.ViewModels.Favorite;
using Microsoft.AspNetCore.Identity;

namespace RealEstateApp.Web.Controllers
{
    public class PropertyController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IPropertyService propertyService;
        private readonly IValidationService validationService;
        private readonly IFavoriteService favoriteService;
        private readonly UserManager<ApplicationUser> userManager;

        public PropertyController(ApplicationDbContext dbContext, IPropertyService propertyService, IValidationService validationService, IFavoriteService favoriteService, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.propertyService = propertyService;
            this.validationService = validationService;
            this.favoriteService = favoriteService;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<PropertyViewModel> allProperties = await propertyService.IndexGetAllPropertiesAsync();
            return View(allProperties);
        }

        // this Get will load the input form. 

        [HttpGet]
        public IActionResult Create()
        {
            return this.View();
        }

        // this submit the form once the submit button is pressed.

        [HttpPost]
        public async Task<IActionResult> Create(PropertyAddFormInputModel propModel)
        {
            //  this.ModelState.IsValid >> will validate the data annotations in the PropertyAddForm
            if (this.ModelState.IsValid == false)
            {
                // Will render the same model plus the validation errors. 
                return View(propModel);
            }

            await this.propertyService
                 .AddProperty
                 (
                     propModel.District,
                     (byte)propModel.Floor!,
                     (byte)propModel.TotalFloors!,
                     propModel.Size,
                     propModel.YardSize,
                     propModel.Year,
                     propModel.PropertyType,
                     propModel.BuildingType,
                     propModel.Price, 
                     propModel.ImageUrl!
                 );

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            bool isIdValid = this.validationService.IsValidGuid(id, out Guid idValid);

            if (isIdValid == false)
            {
                return RedirectToAction(nameof(Index));
            }

            PropertyViewModel? property = await this.propertyService.GetPropertyDetailsByIdAsync(idValid);

            if (property == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(property);
        }
    }
}


