using Microsoft.AspNetCore.Mvc;

namespace RealEstateApp.Web.Areas.Admin.Controllers
{
    public class ManageUsersController : BaseAdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
