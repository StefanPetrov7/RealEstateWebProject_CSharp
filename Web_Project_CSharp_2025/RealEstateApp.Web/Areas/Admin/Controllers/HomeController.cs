using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Web.Controllers;

namespace RealEstateApp.Web.Areas.Admin.Controllers
{
    public class HomeController : BaseAdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
