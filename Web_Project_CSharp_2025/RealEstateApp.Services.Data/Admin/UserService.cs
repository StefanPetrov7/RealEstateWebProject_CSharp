using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Data.DataServices;
using RealEstateApp.Data.DataServices.Contracts;
using RealEstateApp.Data.Models;
using RealEstateApp.Services.Data.Admin.Contracts;
using RealEstateApp.Web.ViewModels.Admin.Users;


namespace RealEstateApp.Services.Data.Admin
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole<Guid>> roleManager;
        private readonly IValidationService validationService;

        public UserService(
            UserManager<ApplicationUser> userManager,
            IValidationService validationService,
            RoleManager<IdentityRole<Guid>> roleManager
            )
        {
            this.userManager = userManager;
            this.validationService = validationService;
            this.roleManager = roleManager;
        }

        public async Task<IEnumerable<UserManagementIndexViewModel>> GetAllUsersAsync(string userId)
        {
            // userId in case we want to do operation with it like to filter and exclude the admin from the list. 

            bool isValidGuid = this.validationService.IsValidGuid(userId, out Guid adminId);

            if (isValidGuid == false)
            {
                throw new ArgumentException($"Invalid admin ID {userId}");
            }

            var users = await this.userManager.Users.ToArrayAsync();

            var result = new List<UserManagementIndexViewModel>();

            foreach (var user in users)
            {
                var roles = await this.userManager.GetRolesAsync(user);

                var userToAdd = new UserManagementIndexViewModel()
                {
                    Id = user.Id.ToString(),
                    UserName = user.UserName,
                    Email = user.Email,
                    IsDeleted = user.IsDeleted,
                    Roles = roles,
                };

                result.Add(userToAdd);
            }

            return result; ;
        }

        public async Task<bool> SoftDeleteUser(string userId)
        {

            bool isValidGuid = this.validationService.IsValidGuid(userId, out Guid userToDeleteId);

            if (isValidGuid == false)
            {
                throw new ArgumentException($"Invalid user to delete ID {userId}");
            }

            var userToDelete = await this.userManager.Users.FirstOrDefaultAsync(x => x.Id == userToDeleteId);

            if (userToDelete == null)
            {
                return false;
            }

            userToDelete.IsDeleted = true;

            var result = await userManager.UpdateAsync(userToDelete);

            return result.Succeeded;

        }

        public async Task<bool> Restore(string userId)
        {

            bool isValidGuid = this.validationService.IsValidGuid(userId, out Guid userToRestoreId);

            if (isValidGuid == false)
            {
                throw new ArgumentException($"Invalid user to restore ID {userId}");
            }

            var UserToRestore = await this.userManager.Users.FirstOrDefaultAsync(x => x.Id == userToRestoreId);

            if (UserToRestore == null)
            {
                return false;
            }

            UserToRestore.IsDeleted = false;

            var result = await userManager.UpdateAsync(UserToRestore);

            return result.Succeeded;

        }

        public async Task<bool> AssignRoleAsync(string userId, string roleId)
        {
            bool isValidUser = this.validationService.IsValidGuid(userId, out Guid userToAssignRoleId);
            bool isValidRole = this.validationService.IsValidGuid(roleId, out Guid roleToBeAssignedId);

            if (isValidUser == false || isValidRole == false)
            {
                return false;
            }

            var user = await this.userManager.FindByIdAsync(userToAssignRoleId.ToString());
            var role = await this.roleManager.FindByIdAsync(roleToBeAssignedId.ToString());

            if (user == null || role == null)
            {
                return false;
            }

            var currentRoles = await this.userManager.GetRolesAsync(user);
            await this.userManager.RemoveFromRolesAsync(user, currentRoles);

            var result = await this.userManager.AddToRoleAsync(user, role.Name);

            return result.Succeeded;

        }
    }
}
