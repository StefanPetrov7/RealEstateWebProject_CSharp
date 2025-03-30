
namespace RealEstateApp.Data.DataServices.Contracts
{
    public interface IValidationService
    {
        bool IsValidGuid(string id, out Guid guidId);
    }
}
