using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Common
{
    // Static class TODO validation constants for the DB models if needed!
    public static class EntityValidationConstants
    {
        public static class Property
        {
            public const int PropertyTypeMinLength = 4;
            public const int PropertyBuildingTypeMinLength = 2;
            public const int PropertyMinPrice = 1;
            public const int PropertyMinSize = 1;
            public const int PropertyMinYardSize = 1;
            public const int PropertyDistrictMinLength = 3;
            public const int PropertyMinYear = 1800;
            public const int PropertyMaxYear = 2030;
            public const int PropertyMinTotalFloors = 1;
            public const int PropertyImageUrlMinLength = 9;
            public const int PropertyImageUrlMaxLength = 2083;

        }

        public static class Favorite
        {
            public const int FavoriteNameMinLength = 4;
            public const int FavoriteNameMaxLength = 20;  
        }
    }
}