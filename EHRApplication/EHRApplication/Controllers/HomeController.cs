using EHRApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EHRApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly LogService _logService;

        public HomeController(LogService logService)
        {
            _logService = logService;
        }

        public IActionResult Index()
        {
            try
            {
                // Example: Log success message if no exception is thrown
                //_logService.WriteToDatabase("Success", "Home page loaded correctly.", "HomeController.cs");
                return View();
            }
            catch (Exception ex)
            {
                // Example: Log error message if an exception occurs
                //_logService.WriteToDatabase("Error", "An error occurred while loading the home page.", ex.Message);
                return View("Error"); // Can display an error view if we make one.
            }
        }


        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Patient()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult LandingPage()
        {
            return View();
        }
    }
}
