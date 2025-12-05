using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace RealEstateApp.Web.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public abstract class BaseController : Controller
    {
        protected bool IsUserAuthenticated()
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
