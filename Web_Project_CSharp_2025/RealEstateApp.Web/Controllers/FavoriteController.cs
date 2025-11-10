using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Data;
using RealEstateApp.Data.DataServices.Contracts;
using RealEstateApp.Data.Models;
using RealEstateApp.Data.Repository.Contracts;
using RealEstateApp.Web.ViewModels.Favorite;
using RealEstateApp.Web.ViewModels.Property;

namespace RealEstateApp.Web.Controllers
{
    public class FavoriteController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFavoriteService favoriteService;
        private readonly IValidationService validationService;
        private readonly IRepository<Favorite, Guid> favoriteRepository;
        private readonly IRepository<Property, Guid> propertyRepository;

        public FavoriteController(
            ApplicationDbContext dbContext,
            IFavoriteService favoriteService,
            IValidationService validationService,
            UserManager<ApplicationUser> userManager,
            IRepository<Favorite, Guid> favRepo,
            IRepository<Property, Guid> propRepository,
            IRepository<PropertyFavorite, Guid> propFavRepository
            )
        {
            this.favoriteService = favoriteService;
            this.validationService = validationService;
            this.userManager = userManager;
            this.favoriteRepository = favRepo;
            this.propertyRepository = propRepository;
        }

        public async Task<IActionResult> Index()
        {
            string userId = this.userManager.GetUserId(this.User)!;

            if (userId == null)
            {
                return this.Forbid();
            }

            IEnumerable<FavoriteView> favoriteViewModels = await favoriteService.IndexGetAllFavoritesAsync(userId);

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
                return Forbid();
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

            FavoritePropertyViewModel? favProperties = await this.favoriteService.GetFavoriteDetailsAsync(favGuid);

            if (favProperties == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(favProperties);

        }

        [HttpGet]
        public async Task<IActionResult> AddPropertyToFavorites(string id)
        {

            if (this.validationService.IsValidGuid(id, out Guid propIdGuid) == false)
            {
                return RedirectToAction(nameof(Index));
            }

            var userId = Guid.Parse(userManager.GetUserId(this.User)!);

            Property? property = await this.propertyRepository.GetByIdAsync(propIdGuid);

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
                Favorites = await this.favoriteRepository.GetAllAttached()
                .Where(x => x.UserId == userId)
                .Include(x => x.FavoriteProperties)!
                .ThenInclude(x => x.Property)
                .Select(x => new FavoriteCheckBoxInputModel()
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    IsSelected = x.FavoriteProperties!.Any(x => x.Property.Id == propIdGuid && x.IsDeleted == false),
                })
                .ToArrayAsync()
            };

            return this.View(propAddToFavModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddPropertyToFavorites(PropertyAddToFavoritesModel propAddToFavModel)
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

            Property? propertyExists = await this.propertyRepository.GetByIdAsync(validPropId);

            if (propertyExists == null)
            {
                return this.RedirectToAction(nameof(Index));
            }

            var favoritesIds = new List<Guid>();

            foreach (FavoriteCheckBoxInputModel selectedFavorite in propAddToFavModel.Favorites)
            {
                if (selectedFavorite.IsSelected == true && this.validationService.IsValidGuid(selectedFavorite.Id, out Guid validId))
                {
                    favoritesIds.Add(validId);
                }
            }

            if (await this.favoriteService.AddPropertyToFavoritesAsync(validPropId, favoritesIds) == false)
            {
                return this.RedirectToAction(nameof(Index));
            }

            return this.RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public async Task<IActionResult> RemovePropertyFromFavorites(string propId, string favoriteId)
        {
            bool isPropGuidValid = this.validationService.IsValidGuid(propId, out Guid validPropId);
            bool isFavGuidValid = this.validationService.IsValidGuid(favoriteId, out Guid validFavoriteId);

            if (isPropGuidValid == false || isFavGuidValid == false)
            {
                return this.RedirectToAction(nameof(Index));
            }

            Guid validUserId = Guid.Parse(this.userManager.GetUserId(User)!);

            bool isRemoved = await this.favoriteService.RemovePropertyFromFavoriteAsync(validPropId,validFavoriteId,validUserId);

            if (isRemoved == false)
            {
                return this.RedirectToAction(nameof(Index));
            }

            return this.RedirectToAction(nameof(Details), new { id = favoriteId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFavorite(string id)
        {
            bool isFavoriteIdValid = this.validationService.IsValidGuid(id, out Guid favoriteId);

            if (isFavoriteIdValid == false)
            {
                return this.RedirectToAction(nameof(Index));
            }

            var isDeleted = await this.favoriteService.SoftDeleteFavoriteAsync(favoriteId);

            if (isDeleted == false)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
