using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using RealEstateApp.Data;
using RealEstateApp.Data.DataServices.Contracts;
using RealEstateApp.Web.ViewModels.Property;

namespace RealEstateApp.Web.Controllers
{
    // TODO make the controller async.
    public class PropertyController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IPropertyService propertyService;

        public PropertyController(ApplicationDbContext dbContext, IPropertyService propertyService)
        {
            this.dbContext = dbContext;
            this.propertyService = propertyService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<PropertyViewModel> allProperties = dbContext.Properties
                .Include(x => x.PropertyType)
                .Include(x => x.BuildingType)
                .Include(x => x.District)
                .Select(x => new PropertyViewModel
                {
                    Id = x.Id.ToString(),
                    Name = x.PropertyType.Name,
                    BuildingType = x.BuildingType.Name,
                    DistrictName = x.District.Name,
                    Floor = x.Floor,
                    Price = x.Price,
                    Size = x.Size,
                    Year = x.Year,
                    DateAdded = x.DateAdded,
                })
                .ToArray();

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
        public IActionResult Create(PropertyAddFormInputModel propModel)
        {
            //  this.ModelState.IsValid >> will validate the data annotations in the PropertyAddForm
            if (this.ModelState.IsValid == false)
            {
                // Will render the same model plus the validation errors. 
                return View(propModel); 
            }

            this.propertyService
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
                propModel.Price
                );

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            bool isIdValid = Guid.TryParse(id, out Guid idValid);

            if (isIdValid == false)
            {
                return RedirectToAction(nameof(Index));
            }

            PropertyViewModel? property = this.dbContext.Properties.Where(x => x.Id == idValid)
                .Select(x => new PropertyViewModel
                {
                    Name = x.PropertyType.Name,
                    BuildingType = x.BuildingType.Name,
                    DistrictName = x.District.Name,
                    Floor = x.Floor,
                    TotalFloors = x.TotalFloors,
                    Price = x.Price,
                    Size = x.Size,
                    YardSize = x.YardSize,  
                    Year = x.Year, 
                    DateAdded = x.DateAdded,
                })
                .FirstOrDefault();

            if (property == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(property);

        }
    }
}