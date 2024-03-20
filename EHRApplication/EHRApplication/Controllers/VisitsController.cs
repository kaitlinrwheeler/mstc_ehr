using EHRApplication.Models;
using EHRApplication.Services;
using EHRApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EHRApplication.Controllers
{
    public class VisitsController : BaseController
    {

        public VisitsController(ILogService logService, IListService listService, IConfiguration configuration)
            : base(logService, listService, configuration)
        {
        }

        public IActionResult VisitDetails(int visitId, int mhn)
        {
            //This will set the banner up and the view model so we can view everything
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = _listService.GetPatientByMHN(mhn);

            //This will grab a list of the care plans from the list services for the patient.
            Visits visit = _listService.GetVisitByVisitId(visitId);

            //This will add all of the data to a view bag that will be grabbed else where to display data correctly.
            viewModel.VisitDetails = visit;
            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = mhn;

            return View(viewModel);
        }
    }
}
