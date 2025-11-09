namespace RealEstateApp.Common
{
    public static class AppConstants
    {
        public const int AppReleaseYear = 2024;
        public const string PropertyDefaultImageUrl = "~/Images/default_property_pic.jpg";
        public const string IsDeletedPropertyName = "IsDeleted";
    }

    public class ExceptionMessages 
    {
        public const string SoftDelete = "Soft delete not supported";
        public const string DatabaseTransactionFailed = "DB Add Property Transaction failed: {0}";
    }
}
