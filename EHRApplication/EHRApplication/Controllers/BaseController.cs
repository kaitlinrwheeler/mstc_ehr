using EHRApplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace EHRApplication.Controllers
{
    public class BaseController : Controller
    {
        private readonly IConfiguration _configuration;
        public readonly string _connectionString;
        public readonly ILogService _logService;
        public readonly IListService _listService;

        public BaseController(ILogService logService, IListService listService, IConfiguration configuration)
        {
            _logService = logService;
            _configuration = configuration;
            _connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            _listService = listService;
        }
    }
}