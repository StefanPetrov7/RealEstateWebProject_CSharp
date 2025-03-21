using Microsoft.AspNetCore.Identity;

namespace RealEstateApp.Data.Models
{
    // Extension of the factory IdentityUser in order to change the Id from string to Guid
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid();   
        }

        // TODO more properties can be added. 
    }
}
