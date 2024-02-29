using EHRApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace EHRApplication.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(PatientContact contact)
        {
            if (ModelState.IsValid)
            {
                return View(contact);
            }
            return View();
        }
    }
}
