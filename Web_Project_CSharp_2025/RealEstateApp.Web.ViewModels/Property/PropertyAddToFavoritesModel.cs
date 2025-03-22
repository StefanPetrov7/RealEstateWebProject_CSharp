using RealEstateApp.Web.ViewModels.Favorite;
using System.ComponentModel.DataAnnotations;

using static RealEstateApp.Common.EntityValidationConstants.Property;
using static RealEstateApp.Common.EntityValidationMessages.Property;


namespace RealEstateApp.Web.ViewModels.Property
{
    public class PropertyAddToFavoritesModel
    {
        public PropertyAddToFavoritesModel()
        {
            this.Favorites = new List<FavoriteCheckBoxInputModel>();    
        }

        [Required]
        public string Id { get; set; } = null!;

        [Required]
        [MinLength(PropertyDistrictMinLength, ErrorMessage = PropertyDistrictErrorMessage)]
        public string District { get; set; } = null!;

        [Required]
        [MinLength(PropertyTypeMinLength, ErrorMessage = PropertyTypeErrorMessage)]
        public string PropertyType { get; set; } = null!;

        [Required]
        [MinLength(PropertyBuildingTypeMinLength, ErrorMessage = PropertyBuildingTypeErrorMessage)]
        public string BuildingType { get; set; } = null!;

        [Required]
        [Range(PropertyMinSize, int.MaxValue, ErrorMessage = PropertySizeErrorMessage)]
        public int Size { get; set; }

        [Range(PropertyMinYardSize, int.MaxValue, ErrorMessage = PropertyYardSizeErrorMessage)]
        public int? YardSize { get; set; }

        public byte? Floor { get; set; }

        [Range(PropertyMinTotalFloors, int.MaxValue, ErrorMessage = PropertyTotalFloorsErrorMessage)]
        public byte? TotalFloors { get; set; }

        [Range(PropertyMinYear, PropertyMaxYear, ErrorMessage = PropertyYearErrorMessage)]
        public int? Year { get; set; }

        [Range(PropertyMinPrice, int.MaxValue, ErrorMessage = PropertyPriceErrorMessage)]
        public int? Price { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime DateAdded { get; set; }

        public IList<FavoriteCheckBoxInputModel> Favorites { get; set; }
    }
}
