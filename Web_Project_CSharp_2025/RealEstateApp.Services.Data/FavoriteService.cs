using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Data.DataServices.Contracts;
using RealEstateApp.Data.Models;
using RealEstateApp.Data.Repository.Contracts;
using RealEstateApp.Web.ViewModels.Favorite;

namespace RealEstateApp.Data.DataServices
{
    public class FavoriteService : IFavoriteService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IRepository<Favorite, Guid> favoriteRepository;
        private readonly UserManager<ApplicationUser> userManager;
        public FavoriteService(ApplicationDbContext dbContext, IRepository<Favorite, Guid> favRepo, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.favoriteRepository = favRepo;
            this.userManager = userManager;
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
    }
}
