using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateApp.Data.Models
{
    // Extension of the factory IdentityUser in order to change the Id from string to Guid
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid();   
            this.Favorites = new HashSet<Favorite>();  
        }

        [InverseProperty(nameof(Favorite.User))]
        public virtual ICollection<Favorite> Favorites { get; set; }

    }
}
