using Microsoft.AspNetCore.Mvc;

namespace EHRApplication.Controllers
{
    public class ResourcesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
