using Microsoft.AspNetCore.Mvc;

namespace EHRApplication.Controllers
{
    public class ResourcesController : Controller
    {
        private readonly LogController _logController;

        public ResourcesController(LogController logController)
        {
            _logController = logController;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
