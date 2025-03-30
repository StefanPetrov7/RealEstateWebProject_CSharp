using RealEstateApp.Data.DataServices.Contracts;


namespace RealEstateApp.Data.DataServices
{
    public class ValidationService : IValidationService
    {
        public bool IsValidGuid(string id, out Guid guidId)
        {
            guidId = Guid.Empty;

            if (string.IsNullOrWhiteSpace(id))
            {
                return false;
            }

            return Guid.TryParse(id, out guidId);
        }
    }
}
