

namespace RealEstateApp.Data.DataServices.Contracts
{
    public interface ISeedService
    {
        Task SeedDefaultProperties();

        Task ImportProperties(string fileLocation);

        Task SeedIdentityAsync();
    }
}
