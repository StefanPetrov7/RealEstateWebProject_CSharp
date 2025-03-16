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
        public void AddTag(string name, int? importance = null)
        {
            Tag tag = new Tag()
            {
                Name = name,
                Importance = importance
            };

            this.dbContext.Tags.Add(tag);   
            this.dbContext.SaveChanges();
        }

        public void TagAllProperties()
        {
            throw new NotImplementedException();
        }
    }
}
