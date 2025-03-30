using RealEstateApp.Web.ViewModels.Favorite;

namespace RealEstateApp.Data.DataServices.Contracts
{
    public interface IFavoriteService
    {
        Task AddFavorite(string name, Guid userId, int? importance = null);

        Task<IEnumerable<FavoriteView>> IndexGetAllFavoritesAsync(string userId);
    }
}
