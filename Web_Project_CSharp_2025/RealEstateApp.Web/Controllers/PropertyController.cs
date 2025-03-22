using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using RealEstateApp.Data;
using RealEstateApp.Data.DataServices.Contracts;
using RealEstateApp.Data.Models;
using RealEstateApp.Web.ViewModels.Property;
using RealEstateApp.Web.ViewModels.Favorite;

namespace RealEstateApp.Web.Controllers
{
    public class PropertyController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IPropertyService propertyService;
        private readonly IValidationService validationService;
        private readonly IFavoriteService favoriteService;



        public PropertyController(ApplicationDbContext dbContext, IPropertyService propertyService, IValidationService validationService, IFavoriteService favoriteService)
        {
            this.dbContext = dbContext;
            this.propertyService = propertyService;
            this.validationService = validationService;
            this.favoriteService = favoriteService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<PropertyViewModel> allProperties = await dbContext.Properties
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
                    ImageUrl = x.ImageUrl,  
                })
                .ToArrayAsync();

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
            bool isIdValid = Guid.TryParse(id, out Guid idValid);

            if (isIdValid == false)
            {
                return RedirectToAction(nameof(Index));
            }

            PropertyViewModel? property = await this.dbContext.Properties.Where(x => x.Id == idValid)
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
                    ImageUrl = x.ImageUrl,  
                })
                .FirstOrDefaultAsync();

            if (property == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(property);

        }

        [HttpGet]
        public async Task<IActionResult> AddToFavorites(string id)
        {
            if (this.validationService.IsValidGuid(id, out Guid propIdGuid ) == false)
            {
                return RedirectToAction(nameof(Index));
            }

            Property? property = await this.dbContext.Properties.Where(x => x.Id == propIdGuid)
                .Include(x => x.PropertyType)
                .Include(x => x.BuildingType)
                .Include(x => x.District)
                .FirstOrDefaultAsync();

            if (property == null)
            {
                return RedirectToAction(nameof(Index));
            }

            PropertyAddToFavoritesModel propAddToFavModel = new PropertyAddToFavoritesModel()
            {
                Id = property.Id.ToString(),
                PropertyType = property.PropertyType?.Name ?? "Unknown",
                BuildingType = property.BuildingType?.Name ?? "Unknown", 
                District = property.District?.Name ?? "Unknown",
                Floor = property.Floor,
                TotalFloors = property.TotalFloors,
                Price = property.Price,
                Size = property.Size,
                YardSize = property.YardSize,
                Year = property.Year,
                DateAdded = property.DateAdded,
                ImageUrl = property.ImageUrl,   
                Favorites = await this.dbContext.Favorites
                .Include(x => x.FavoriteProperties)!
                .ThenInclude(x => x.Property)
                .Select(x => new FavoriteCheckBoxInputModel()
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    IsSelected = x.FavoriteProperties!.Any(x => x.Property.Id == propIdGuid && x.IsDeleted == false),
                }).ToArrayAsync()
            };
               
            return this.View(propAddToFavModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddToFavorites(PropertyAddToFavoritesModel propAddToFavModel)
        {

            if (this.ModelState.IsValid == false)
            {
                return this.View(propAddToFavModel);
            }

            bool isPropGuidValid = this.validationService.IsValidGuid(propAddToFavModel.Id, out Guid validPropId);

            if (isPropGuidValid == false)
            {
                return this.RedirectToAction(nameof(Index));
            }

            Property? property = await this.dbContext.Properties.FirstOrDefaultAsync(x => x.Id == validPropId);

            if (property == null)
            {
                return this.RedirectToAction(nameof(Index));
            }

            ICollection<PropertyFavorite> entitiesToAdd = new List<PropertyFavorite>();   

            foreach (FavoriteCheckBoxInputModel favCheckBoxInputModel in propAddToFavModel.Favorites)
            {

                bool isFavGuidValid = this.validationService.IsValidGuid(favCheckBoxInputModel.Id, out Guid validFavGuid);

                if (isFavGuidValid == false)
                {
                    continue;
                }

                Favorite? favorite = await this.dbContext.Favorites.FirstOrDefaultAsync(x => x.Id == validFavGuid);

                if (favorite == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                PropertyFavorite? propFav = await this.dbContext.PropertyFavorites.FirstOrDefaultAsync(x => x.PropertyId == validPropId && x.FavoriteId == validFavGuid);

                if (favCheckBoxInputModel.IsSelected == true)
                {
                    if (propFav == null)
                    {
                        PropertyFavorite propFavModel = new PropertyFavorite()
                        {
                            PropertyId = validPropId,
                            FavoriteId = validFavGuid,
                        };

                        entitiesToAdd.Add(propFavModel);
                    }
                    else
                    {
                        propFav.IsDeleted = false;
                    }
                }
                else 
                {
                    if (propFav != null) 
                    {
                        propFav.IsDeleted = true;
                    }  

                }

                await this.dbContext.SaveChangesAsync();
            }

            await this.dbContext.PropertyFavorites.AddRangeAsync(entitiesToAdd);
            await this.dbContext.SaveChangesAsync();
            return this.RedirectToAction(nameof(Index));
            
        }
    }
}


