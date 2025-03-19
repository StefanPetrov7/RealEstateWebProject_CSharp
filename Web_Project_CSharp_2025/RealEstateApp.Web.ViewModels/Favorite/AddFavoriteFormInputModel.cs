using System.ComponentModel.DataAnnotations;

using static RealEstateApp.Common.EntityValidationConstants.Favorite;
using static RealEstateApp.Common.EntityValidationMessages.Favorite;


namespace RealEstateApp.Web.ViewModels.Favorite
{
    public class AddFavoriteFormInputModel
    {
        [Required]
        [MinLength(FavoriteNameMinLength, ErrorMessage = FavoriteNameErrorMessage)]
        [MaxLength(FavoriteNameMaxLength, ErrorMessage = FavoriteNameErrorMessage)]
        public string Name { get; set; } = null!;

        public int? Importance { get; set; }

    }
}
