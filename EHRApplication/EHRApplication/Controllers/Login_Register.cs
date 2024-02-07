using Microsoft.AspNetCore.Mvc;

namespace EHRApplication.Controllers
{
    public class Login_Register : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
    }
}
