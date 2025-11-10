using RealEstateApp.Web.ViewModels.Favorite;

namespace RealEstateApp.Data.DataServices.Contracts
{
    public interface IFavoriteService
    {
        Task AddFavorite(string name, Guid userId, int? importance = null);

        Task<IEnumerable<FavoriteView>> IndexGetAllFavoritesAsync(string userId);

        Task<bool> AddPropertyToFavoritesAsync(Guid propertyId, IEnumerable<Guid> favoriteIds);

        Task<bool> SoftDeleteFavoriteAsync(Guid id);

        Task<FavoritePropertyViewModel?> GetFavoriteDetailsAsync(Guid favoriteId);

        Task<bool> RemovePropertyFromFavoriteAsync(Guid propertyId, Guid favoriteId, Guid userId);
    }
}
