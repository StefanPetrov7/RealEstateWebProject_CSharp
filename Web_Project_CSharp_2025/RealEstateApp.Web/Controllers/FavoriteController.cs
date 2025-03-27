using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using RealEstateApp.Data;
using RealEstateApp.Data.DataServices.Contracts;
using RealEstateApp.Data.Models;
using RealEstateApp.Web.ViewModels.Favorite;
using RealEstateApp.Web.ViewModels.Property;

namespace RealEstateApp.Web.Controllers
{
    [Authorize]
    public class FavoriteController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFavoriteService favoriteService;
        private readonly IValidationService validationService;

        public FavoriteController(ApplicationDbContext dbContext, IFavoriteService favoriteService, IValidationService validationService, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.favoriteService = favoriteService;
            this.validationService = validationService;
            this.userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            string userId = this.userManager.GetUserId(this.User)!;

            IEnumerable<FavoriteView> favoriteViewModels = await this.dbContext.Favorites
                .Where(x => x.UserId.ToString().ToLower() == userId.ToLower())
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

            var userIdString = userManager.GetUserId(this.User);

            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            var userId = Guid.Parse(userIdString);

            await this.favoriteService.AddFavorite(favoriteFormModel.Name, userId, favoriteFormModel.Importance);
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

        [HttpGet]
        public async Task<IActionResult> AddToFavorites(string id)
        {
            if (this.validationService.IsValidGuid(id, out Guid propIdGuid) == false)
            {
                return RedirectToAction(nameof(Index));
            }

            var userId = Guid.Parse(userManager.GetUserId(this.User)!);

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
                .Where(x => x.UserId == userId)
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

        [HttpPost]
        public async Task<IActionResult> RemoveFromFavorites(string propId)
        {
            bool isPropGuidValid = this.validationService.IsValidGuid(propId, out Guid validPropId);

            if (isPropGuidValid == false)
            {
                return this.RedirectToAction(nameof(Index));
            }

            Property? property = await this.dbContext.Properties.FirstOrDefaultAsync(x => x.Id == validPropId);

            if (property == null)
            {
                return this.RedirectToAction(nameof(Index));
            }

            Guid userId = Guid.Parse(this.userManager.GetUserId(User)!);

            Favorite? favorite = await this.dbContext.Favorites.Where(x => x.UserId == userId).FirstOrDefaultAsync()!;

            if (favorite == null)
            {
                return this.RedirectToAction(nameof(Details));
            }

            PropertyFavorite? propFav = await this.dbContext.PropertyFavorites.FirstOrDefaultAsync(x => x.PropertyId == validPropId && x.FavoriteId == favorite.Id);

            if (propFav == null)
            {
                return this.RedirectToAction(nameof(Index));
            }

            propFav.IsDeleted = true;

            await this.dbContext.SaveChangesAsync();
            return this.RedirectToAction(nameof(Index));

        }
    }
}
