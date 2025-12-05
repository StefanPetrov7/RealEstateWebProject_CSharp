using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateApp.Data.Models
{
    // Extension of the factory IdentityUser in order to change the Id from string to Guid
    // Identity User represent AspNetUsers in the DB scheme
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid();   
            this.Favorites = new HashSet<Favorite>();  
        }

        public bool IsDeleted { get; set; } = false;

        [InverseProperty(nameof(Favorite.User))]
        public virtual ICollection<Favorite> Favorites { get; set; }

    }
}
