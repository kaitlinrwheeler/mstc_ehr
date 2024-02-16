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
            _logService.WriteToDatabase("Success", "Home page loaded correctly.", "HomeController.cs");
            return View();
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
    }
}
