

namespace RealEstateApp.Web.ViewModels.Admin.Users
{
    public class UserManagementIndexViewModel
    {
        public string Id { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public bool IsDeleted { get; set; } = false; 

        public IEnumerable<string> Roles { get; set; } = null!;

    }
}
