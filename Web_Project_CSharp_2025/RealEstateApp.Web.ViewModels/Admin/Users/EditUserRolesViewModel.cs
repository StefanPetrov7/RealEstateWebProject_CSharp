using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RealEstateApp.Web.ViewModels.Admin.Users
{
    public class EditUserRolesViewModel
    {
        public string Id { get; set; } = null!;

        public string Email { get; set; } = null!;

        public List<string> CurrentRoles { get; set; } = new List<string>();

        [Required(ErrorMessage = "Please select a role.")]
        public string SelectedRoleId { get; set; } = string.Empty;

        public List<string> AvailableRoles { get; set; } = new List<string>();
    }
}
