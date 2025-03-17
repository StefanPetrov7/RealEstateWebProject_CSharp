using RealEstateApp.Data.DataServices.Contracts;
using RealEstateApp.Data.Models;


namespace RealEstateApp.Data.DataServices
{
    public class TagService : ITagService
    {
        private readonly ApplicationDbContext dbContext;
        public TagService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task AddTag(string name, int? importance = null)
        {

            Tag tag = new Tag()
            {
                Name = name,
                Importance = importance
            };

            await this.dbContext.Tags.AddAsync(tag);
            await this.dbContext.SaveChangesAsync();
        }

        public Task TagAllProperties()
        {
            throw new NotImplementedException();
        }
    }
}
