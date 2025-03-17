using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Common
{
    public static class EntityValidationMessages
    {
        public static class Property 
        {
            public const string PropertySizeErrorMessage = "Size must be at least 1.";
            public const string PropertyYardSizeErrorMessage = "Yard Size must be at least 1.";
            public const string PropertyTotalFloorsErrorMessage = "Total Floors must be at least 1.";
            public const string PropertyYearErrorMessage = "Year must be between 1800 and 2030.";
            public const string PropertyPriceErrorMessage = "Price must be at least 1 Euro";
            public const string PropertyDistrictErrorMessage = "Property Type length must be min 3 symbols.";
            public const string PropertyTypeErrorMessage = "Property Type length must be min 4 symbols.";
            public const string PropertyBuildingTypeErrorMessage = "Building Type length must be at least 2.";
        }

        public static class Tag
        {
            public const string TagNameErrorMessage = "Name length must be between 4 and 20 symbols.";
        }
    }
}
