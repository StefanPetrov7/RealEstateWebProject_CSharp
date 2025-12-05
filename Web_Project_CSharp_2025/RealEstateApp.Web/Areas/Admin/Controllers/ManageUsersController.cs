using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Data.DataServices.Contracts;
using RealEstateApp.Services.Data.Admin.Contracts;
using RealEstateApp.Web.ViewModels.Admin.Users;
using System.Threading.Tasks;

namespace RealEstateApp.Web.Areas.Admin.Controllers
{
    public class ManageUsersController : BaseAdminController
    {
        private readonly IUserService userService;
        private readonly IValidationService validationService;


        public ManageUsersController(IUserService userService, IValidationService validationService)
        {
            this.userService = userService;
            this.validationService = validationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string? userId = this.GetUserId();

            IEnumerable<UserManagementIndexViewModel> users = await this.userService.GetAllUsersAsync(userId);

            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id) 
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("User ID is required.");
            }

            bool success = await this.userService.SoftDeleteUser(id);

            if (!success)
            {
                return NotFound("User not found or delete failed.");
            }

            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public async Task<IActionResult> Restore(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("User ID is required.");
            }

            bool success = await this.userService.Restore(id);

            if (!success)
            {
                return NotFound("User not found or restore failed.");
            }

            return RedirectToAction(nameof(Index));

        }
    }
}
