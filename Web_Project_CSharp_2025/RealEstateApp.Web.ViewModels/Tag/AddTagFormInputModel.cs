using System.ComponentModel.DataAnnotations;

using static RealEstateApp.Common.EntityValidationConstants.Tag;
using static RealEstateApp.Common.EntityValidationMessages.Tag;


namespace RealEstateApp.Web.ViewModels.Tag
{
    public class AddTagFormInputModel
    {
        [Required]
        [MinLength(TagNameMinLength, ErrorMessage = TagNameErrorMessage)]
        [MaxLength(TagNameMaxLength, ErrorMessage = TagNameErrorMessage)]
        public string Name { get; set; } = null!;

        public int? Importance { get; set; }

    }
}
