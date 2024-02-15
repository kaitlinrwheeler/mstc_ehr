using EHRApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EHRApplication.Controllers
{
    public class PatientController : Controller
    {
        private readonly LogController _logController;

        public PatientController(LogController logController)
        {
            _logController = logController;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
