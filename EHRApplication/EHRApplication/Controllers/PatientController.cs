using EHRApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EHRApplication.Controllers
{
    public class PatientController : Controller
    {
        private readonly LogService _logService;

        public PatientController(LogService logService)
        {
            _logService = logService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
