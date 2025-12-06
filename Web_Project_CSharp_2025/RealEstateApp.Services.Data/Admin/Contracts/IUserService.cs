using RealEstateApp.Web.ViewModels.Admin.Users;

namespace RealEstateApp.Services.Data.Admin.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserManagementIndexViewModel>> GetAllUsersAsync(string userId);

        Task<bool> SoftDeleteUser(string userId);

        Task<bool> Restore(string userId);

        Task<bool> AssignRoleAsync(string userId, string roleId);

    }
}
