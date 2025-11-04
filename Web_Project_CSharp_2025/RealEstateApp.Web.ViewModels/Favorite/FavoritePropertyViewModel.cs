using RealEstateApp.Web.ViewModels.Property;

namespace RealEstateApp.Web.ViewModels.Favorite
{
    public class FavoritePropertyViewModel
    {
        public FavoritePropertyViewModel()
        {
            this.Properties = new HashSet<PropertyViewModel>();
        }

        public Guid Id   { get; set; }

        public string Name { get; set; } = null!;

        public ICollection<PropertyViewModel> Properties { get; set; }
    }
}
