using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RealEstateApp.Common;
using System.Security.Claims;

namespace RealEstateApp.Web.Areas.Admin.Controllers
{
    [Area(AppConstants.AdminAreaName)]
    [Authorize(Roles = AppConstants.AdminRoleName)]
    public abstract class BaseAdminController : Controller
    {
        private bool IsUserAuthenticated()
        {
            bool result = false;

            if (this.User.Identity != null)
            {
                result = this.User.Identity.IsAuthenticated;
            }

            return result;
        }

        protected string? GetUserId()
        {
            string userId = null;

            if (this.IsUserAuthenticated())
            {
                userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            return userId;
        }
    }
}

