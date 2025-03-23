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
        public async Task AddFavorite(string name, Guid userId ,int? importance = null)
        {

            Favorite favorite = new Favorite()
            {
                Name = name,
                Importance = importance,
                UserId = userId 
            };

            await this.dbContext.Favorites.AddAsync(favorite);
            await this.dbContext.SaveChangesAsync();
        }

      
    }
}
