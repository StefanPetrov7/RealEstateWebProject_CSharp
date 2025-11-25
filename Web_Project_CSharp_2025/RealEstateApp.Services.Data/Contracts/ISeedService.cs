

namespace RealEstateApp.Data.DataServices.Contracts
{
    public interface ISeedService
    {
        Task RunSeed();

        Task ImportProperties(string fileLocation);

         void SeedIdentity(IServiceProvider serviceProvider);
    }
}
