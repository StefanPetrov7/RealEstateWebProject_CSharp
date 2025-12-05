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
        private readonly IRepository<Favorite, Guid> favoriteRepository;
        private readonly IRepository<Property, Guid> propertyRepository;
        private readonly IRepository<PropertyFavorite, Guid> propFavRepository;
        private readonly UserManager<ApplicationUser> userManager;


        public FavoriteService(
            IRepository<Favorite, Guid> favRepo,
            UserManager<ApplicationUser> userManager,
            IRepository<Property, Guid> propRepo,
            IRepository<PropertyFavorite, Guid> propFavRepository)
        {
            this.favoriteRepository = favRepo;
            this.userManager = userManager;
            this.propertyRepository = propRepo;
            this.propFavRepository = propFavRepository;
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

        public async Task<IEnumerable<FavoriteView>> IndexGetAllFavoritesAsync(string userId)
        {
            IEnumerable<FavoriteView> favoriteViewModels = await this.favoriteRepository.GetAllAttached()
                 .Where(x => x.UserId.ToString().ToLower() == userId.ToLower()
                 && x.IsDeleted == false
                 && x.User.IsDeleted == false)
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

            Property? propertyExists = await this.propertyRepository.GetByIdAsync(propertyId);

            if (propertyExists == null)
            {
                return false;
            }

            ICollection<PropertyFavorite> entitiesToAdd = new List<PropertyFavorite>();

            foreach (Guid favoriteId in favoriteIds)
            {
                Favorite? favorite = await this.favoriteRepository.GetByIdAsync(favoriteId);

                if (favorite == null)
                {
                    continue;
                }

                PropertyFavorite? propFav = await this.propFavRepository
                    .GetAllAttached()
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

                    entitiesToAdd.Add(propFavModel);
                }
                else if (propFav.IsDeleted)
                {
                    propFav.IsDeleted = false;
                    await propFavRepository.UpdateAsync(propFav);
                }
            }

            if (entitiesToAdd.Any())
            {
                await this.propFavRepository.AddRangeAsync(entitiesToAdd);
            }

            return true;
        }

        public async Task<bool> SoftDeleteFavoriteAsync(Guid id)
        {
            Favorite? favoriteToDelete = await this.favoriteRepository.FirstOrDefaultAsync(x => x.Id == id);

            if (favoriteToDelete == null)
            {
                return false;
            }

            await this.favoriteRepository.DeleteAsync(favoriteToDelete);
            return true;
        }

        public async Task<FavoritePropertyViewModel?> GetFavoriteDetailsAsync(Guid favoriteId)
        {
            FavoritePropertyViewModel? favoriteProperty = await this.favoriteRepository
                .GetAllAttached()
                .Where(x => x.Id == favoriteId && x.IsDeleted == false && x.User.IsDeleted == false)
                .Select(x => new FavoritePropertyViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Properties = x.FavoriteProperties!
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new PropertyViewModel
                    {
                        Id = x.PropertyId.ToString(),
                        Name = x.Property.PropertyType.Name,
                        BuildingType = x.Property.BuildingType.Name,
                        DistrictName = x.Property.District.Name,
                        Floor = x.Property.Floor,
                        Price = x.Property.Price,
                        Year = x.Property.Year,
                        DateAdded = x.Property.DateAdded,
                    }).ToArray()
                }).FirstOrDefaultAsync();

            return favoriteProperty;
        }

        public async Task<bool> RemovePropertyFromFavoriteAsync(Guid propertyId, Guid favoriteId, Guid userId)
        {
            bool propertyExists = await this.propertyRepository.GetAllAttached()
                .AnyAsync(x => x.Id == propertyId);

            if (propertyExists == false)
            {
                return false;
            }

            PropertyFavorite? propFav = await this.propFavRepository
            .GetAllAttached()
            .Include(x => x.Favorite)
            .FirstOrDefaultAsync(x => x.PropertyId == propertyId && x.Favorite.UserId == userId && x.FavoriteId == favoriteId);

            if (propFav == null)
            {
                return false;
            }

            propFav.IsDeleted = true;
            await this.propFavRepository.UpdateAsync(propFav);
            return true;

        }

        // TODO Below is not tested and optional
        //public async Task<bool> RestoreFavoriteAsync(Guid id)
        //{
        //    Favorite? favorite = await this.favoriteRepository
        //        .GetAllAttached()
        //        .IgnoreQueryFilters()
        //        .FirstOrDefaultAsync(x => x.Id == id);

        //    if (favorite == null || !favorite.IsDeleted)
        //    {
        //        return false;
        //    }

        //    favorite.IsDeleted = false;
        //    return await this.favoriteRepository.UpdateAsync(favorite);
        //}
    }
}
