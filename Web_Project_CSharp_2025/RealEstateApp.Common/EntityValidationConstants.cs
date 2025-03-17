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
        }

        public static class Tag
        {
            public const int TagNameMinLength = 4;
            public const int TagNameMaxLength = 20;  
        }
    }
}