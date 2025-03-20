using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using RealEstateApp.Data;
using RealEstateApp.Data.DataServices.Contracts;
using RealEstateApp.Web.ViewModels.Favorite;
using RealEstateApp.Web.ViewModels.Property;


namespace RealEstateApp.Web.Controllers
{
    public class FavoriteController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IFavoriteService favoriteService;
        private readonly IValidationService validationService;

        public FavoriteController(ApplicationDbContext dbContext, IFavoriteService favoriteService, IValidationService validationService)
        {
            this.dbContext = dbContext;
            this.favoriteService = favoriteService;
            this.validationService = validationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<FavoriteView> favoriteViewModels = await this.dbContext.Favorites
                .Select(x => new FavoriteView()
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    Importance = x.Importance,
                })
                .ToArrayAsync();

            return this.View(favoriteViewModels);
        }

        // HttpGet >> return the view with the form.
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddFavoriteFormInputModel favoriteFormModel)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View(favoriteFormModel);
            }

            await this.favoriteService.AddFavorite(favoriteFormModel.Name, favoriteFormModel.Importance);
            return this.RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {

            if (this.validationService.IsValidGuid(id!, out Guid favGuid) == false)
            {
                return RedirectToAction(nameof(Index));
            }

            FavoritePropertyViewModel? favProperties = await this.dbContext.Favorites.Where(x => x.Id == favGuid)
                .Include(x => x.FavoriteProperties)!
                .ThenInclude(x => x.Property)
                .Select(x => new FavoritePropertyViewModel()
                {
                    Name = x.Name,
                    Properties = x.FavoriteProperties!
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new PropertyViewModel
                    {
                        Id = x.Property.Id.ToString(),
                        Name = x.Property.PropertyType.Name,
                        BuildingType = x.Property.BuildingType.Name,
                        DistrictName = x.Property.District.Name,
                        Floor = x.Property.Floor,
                        Price = x.Property.Price,
                        Size = x.Property.Size,
                        Year = x.Property.Year,
                        DateAdded = x.Property.DateAdded,
                    }).ToArray()
                }).FirstOrDefaultAsync();

            if (favProperties == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(favProperties);

        }


    }
}
