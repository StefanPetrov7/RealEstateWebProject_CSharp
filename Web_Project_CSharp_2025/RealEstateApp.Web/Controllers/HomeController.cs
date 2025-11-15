using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RealEstateApp.Web.ViewModels;

namespace RealEstateApp.Web.Controllers
{
    public class HomeController : BaseController
    {

        public HomeController()
        {

        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            ViewData["Title"] = "Home Page";
            ViewData["Message"] = "Welcome to RealEstate App!";

            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }


        // Will not catch 401!
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode)
        {

            ViewData["StatusCode"] = statusCode;

            switch (statusCode)
            {
                case null:
                    return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                case 401:
                case 403:
                case 404:
                    return View("UnauthorizedError");

                default:
                    return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

            }
        }
    }
}