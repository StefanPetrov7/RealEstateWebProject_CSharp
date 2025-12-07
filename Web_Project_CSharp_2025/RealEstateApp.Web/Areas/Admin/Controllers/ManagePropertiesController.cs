using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using RealEstateApp.Data.DataServices.Contracts;
using RealEstateApp.Web.ViewModels.Property;

namespace RealEstateApp.Web.Areas.Admin.Controllers
{
    
    public class ManagePropertiesController : BaseAdminController
    {

        private readonly IPropertyService propertyService;
        private readonly IValidationService validationService;



        public ManagePropertiesController
            (
            IPropertyService propertyService, 
            IValidationService validationService    
            )
        {
            this.propertyService = propertyService;
            this.validationService = validationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<PropertyViewModel> properties = await this.propertyService.IndexGetAllPropertiesAsync();
            return View(properties);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id) 
        {
            bool isValidId = this.validationService.IsValidGuid(id, out Guid propertyId);

            if (isValidId == false)
            {
                return NotFound();
            }

            PropertyViewModel property = await this.propertyService.GetPropertyDetailsByIdAsync(propertyId);

            if (property == null)
            {
                return NotFound();
            }

            return View(property);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PropertyViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            bool isValidId = this.validationService.IsValidGuid(model.Id, out Guid propertyId);

            if (isValidId == false)
            {
                return NotFound();
            }

            var success = await this.propertyService.UpdatePropertyAsync(model);

            if (success == false)
            {
                ModelState.AddModelError("", "Unable to update property!");
                return View(model);
            }

            return RedirectToAction("Index");

        }

        [HttpGet]
        public  IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PropertyAddFormInputModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            string district = model.District;
            byte? floor = model.Floor;
            byte? totalFloors = model.TotalFloors;
            int size = model.Size;
            int? yardSize = model.YardSize;
            int? year = model.Year;
            string propertyType = model.PropertyType;
            string buildingType = model.BuildingType;
            int? price = model.Price;
            string? imageUrl = model.ImageUrl;

            await this.propertyService.AddPropertyAsync(district, floor, totalFloors, size, yardSize, year, propertyType, buildingType, price, imageUrl);

            return RedirectToAction(nameof(Index));

        }





    }
}
