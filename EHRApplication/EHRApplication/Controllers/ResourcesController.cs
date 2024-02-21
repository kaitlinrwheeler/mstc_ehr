using EHRApplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace EHRApplication.Controllers
{
    public class ResourcesController : Controller
    {
        private readonly LogService _logService;

        public ResourcesController(LogService logService)
        {
            _logService = logService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
