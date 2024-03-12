using EHRApplication.Models;
using EHRApplication.Services;
using EHRApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EHRApplication.Controllers
{
    public class LabController : Controller
    {
        private readonly LogService _logService;
        //Setting the default connection string
        private string connectionString;
        public IConfiguration Configuration { get; }

        public LabController(LogService logService, IConfiguration configuration)
        {
            _logService = logService;
            Configuration = configuration;
            this.connectionString = Configuration["ConnectionStrings:DefaultConnection"];
        }

        public IActionResult LabOrders(int mhn)
        {
            //This will set the banner up and the view model so we can view everything
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = new ListService(Configuration).GetPatientByMHN(mhn);

            //Calls the list service to get all of the Lab orders associated to the passed in mhn number.
            List<LabOrders> labOrders = new ListService(Configuration).GetPatientsLabOrdersByMHN(mhn);

            //This will add the patient lab orders to the view model so it can be displayed along with the banner at the same time.
            viewModel.LabOrders = labOrders;

            //This will add all of the data to a view bag the will be grabbed else where to display data correctly.
            ViewBag.Patient = viewModel.LabOrders;
            ViewBag.MHN = mhn;

            return View(viewModel);
        }
    }
}
