using RealEstateApp.Data.DataServices.Contracts;
using RealEstateApp.Data.Models;


namespace RealEstateApp.Data.DataServices
{
    public class FavoriteService : IFavoriteService
    {
        private readonly ApplicationDbContext dbContext;
        public FavoriteService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task AddFavorite(string name, int? importance = null)
        {

            Favorite favorite = new Favorite()
            {
                Name = name,
                Importance = importance
            };

            await this.dbContext.Favorites.AddAsync(favorite);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task AddPropertyToFavorites(Guid propGuid, Guid favGuid)
        {
            PropertyFavorite propertyFavorite = new PropertyFavorite()
            {
                PropertyId = propGuid,
                FavoriteId = favGuid,
            };

            await this.dbContext.PropertyFavorites.AddAsync(propertyFavorite);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
