using EHRApplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace EHRApplication.Controllers
{
    public class ResourcesController : BaseController
    {
        private readonly ILogService _logService;
        private readonly string _connectionString;
        private readonly IListService _listService;

        public ResourcesController(ILogService logService, IListService listService, IConfiguration configuration)
            : base(logService, listService, configuration)
        {
            _logService = logService;
            this._connectionString = base._connectionString;
            _listService = listService;
    }

        public IActionResult Index()
        {
            return View();
        }
    }
}
