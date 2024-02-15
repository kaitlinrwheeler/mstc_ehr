using EHRApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace EHRApplication.Controllers
{
    public class LogController : Controller
    {
        private readonly LogService _logService;

        public LogController(LogService logService)
        {
            _logService = logService;
        }

        public IActionResult WriteToDatabase()
        {
            // Example usage:
            Log log = new Log
            {
                Severity = "Error",
                Message = "An error occurred",
                Context = "{}", // Empty context, as users don't directly provide context
                DateAndTime = DateTime.Now // Current date and time
            };

            try
            {
                _logService.WriteToDatabase(log);
                return Ok("Log written successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Failed to write log to database: " + ex.Message);
            }
        }
    }
}
