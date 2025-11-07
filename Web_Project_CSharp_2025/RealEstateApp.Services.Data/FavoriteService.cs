using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Data.DataServices.Contracts;
using RealEstateApp.Data.Models;
using RealEstateApp.Data.Repository.Contracts;
using RealEstateApp.Web.ViewModels.Favorite;
using RealEstateApp.Web.ViewModels.Property;
using System.Reflection.Metadata.Ecma335;

namespace RealEstateApp.Data.DataServices
{
    public class FavoriteService : IFavoriteService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IRepository<Favorite, Guid> favoriteRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IValidationService validationService;
        public FavoriteService(ApplicationDbContext dbContext, IRepository<Favorite, Guid> favRepo, UserManager<ApplicationUser> userManager, IValidationService validationService)
        {
            this.dbContext = dbContext;
            this.favoriteRepository = favRepo;
            this.userManager = userManager;
            this.validationService = validationService;
        }

        public async Task AddFavorite(string name, Guid userId, int? importance = null)
        {
            Favorite favorite = new Favorite()
            {
                Name = name,
                Importance = importance,
                UserId = userId
            };

            await this.favoriteRepository.AddAsync(favorite);
        }

        // TODO Filter by IsDeleted 
        public async Task<IEnumerable<FavoriteView>> IndexGetAllFavoritesAsync(string userId)
        {

            IEnumerable<FavoriteView> favoriteViewModels = await this.favoriteRepository.GetAllAttached()
                 .Where(x => x.UserId.ToString().ToLower() == userId.ToLower())
                 .Select(x => new FavoriteView()
                 {
                     Id = x.Id.ToString(),
                     Name = x.Name,
                     Importance = x.Importance,
                 })
                 .ToArrayAsync();

            return favoriteViewModels;
        }

        public async Task<bool> AddPropertyToFavoritesAsync(Guid propertyId, IEnumerable<Guid> favoriteIds)
        {

            bool propertyExists = await this.dbContext.Properties.AnyAsync(x => x.Id == propertyId);

            if (propertyExists == false)
            {
                return false;
            }

            ICollection<PropertyFavorite> entitiesToAdd = new List<PropertyFavorite>();

            foreach (Guid favoriteId in favoriteIds)
            {

                bool favoriteExists = await this.dbContext.Favorites.AnyAsync(x => x.Id == favoriteId);

                if (favoriteExists == false)
                {
                    continue;
                }

                PropertyFavorite? propFav = await this.dbContext.PropertyFavorites
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(x => x.PropertyId == propertyId && x.FavoriteId == favoriteId);

                if (propFav == null)
                {
                    PropertyFavorite propFavModel = new PropertyFavorite()
                    {
                        PropertyId = propertyId,
                        FavoriteId = favoriteId,
                        IsDeleted = false,
                    };

                    await this.dbContext.PropertyFavorites.AddAsync(propFavModel);

                }
                else
                {
                    if (propFav.IsDeleted)
                    {
                        propFav.IsDeleted = false;
                    }
                }
            }

            await this.dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftDeleteFavoriteAsync(Guid id)
        {
            Favorite? favoriteToDelete = await this.dbContext.Favorites.FirstOrDefaultAsync(x => x.Id == id);

            if (favoriteToDelete == null) 
            { 
                return false;
            }

            favoriteToDelete.IsDeleted = true;
            await this.dbContext.SaveChangesAsync();    
            return true;
        }
    }

}
