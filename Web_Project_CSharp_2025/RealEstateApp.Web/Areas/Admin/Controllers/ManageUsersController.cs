using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Data.DataServices.Contracts;
using RealEstateApp.Data.Models;
using RealEstateApp.Services.Data.Admin.Contracts;
using RealEstateApp.Web.ViewModels.Admin.Users;
using System.Threading.Tasks;

namespace RealEstateApp.Web.Areas.Admin.Controllers
{
    public class ManageUsersController : BaseAdminController
    {
        private readonly IUserService userService;
        private readonly IValidationService validationService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole<Guid>> roleManager;


        public ManageUsersController(
            IUserService userService,
            IValidationService validationService,
            RoleManager<IdentityRole<Guid>> roleManager,
            UserManager<ApplicationUser> userManager
            )
        {
            this.userService = userService;
            this.validationService = validationService;
            this.userManager = userManager;
            this.roleManager = roleManager;
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

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var isValidId = this.validationService.IsValidGuid(id, out Guid userId);

            if (isValidId == false)
            {
                return BadRequest("Invalid user Id");
            }

            var user = await this.userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await this.userManager.GetRolesAsync(user);

            var allRoles = await this.roleManager.Roles.Select(x => new
            {
                x.Id,
                x.Name,
            }).ToListAsync();

            var roleOptions = allRoles.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
                Selected = userRoles.Contains(x.Name)
            }).ToList();

            EditUserRolesViewModel usersRolesViewModel = new EditUserRolesViewModel
            {
                Id = user.Id.ToString(),
                Email = user.Email ?? user.UserName,
                CurrentRoles = userRoles.ToList(),
                SelectedRoleId = roleOptions.FirstOrDefault(x => x.Selected)?.Value ?? string.Empty,
                AvailableRoles = allRoles.Select(x => $"{x.Id}|{x.Name}").ToList()
            };

            ViewData["RoleOptions"] = roleOptions;

            return View(usersRolesViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserRolesViewModel model)
        {
            var success = await this.userService.AssignRoleAsync(model.Id, model.SelectedRoleId);

            if (success == false)
            {
                ModelState.AddModelError(string.Empty, "Failed to assign role!");

                var allRoles = await roleManager.Roles
                    .Select(x => new
                    {
                        x.Id,
                        x.Name
                    }).ToListAsync();

                var roleOptions = allRoles.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                    Selected = x.Id.ToString() == model.SelectedRoleId
                }).ToList();

                ViewData["RoleOptions"] = roleOptions;
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
