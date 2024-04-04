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

            //This will the specific visit
            viewModel.VisitDetails = _listService.GetVisitByVisitId(visitId);

            //This will grab the lab results associated with the visit.
            viewModel.LabResultsDetails = _listService.GetLabResultsByVisitId(visitId);
            //This will grab the lab order associated with the visit.
            viewModel.LabOrdersDetails = _listService.GetLabOrdersByVisitId(visitId);
            //This will grab the med order associated with the visit.
            viewModel.MedOrdersDetails = _listService.GetMedOrdersByVisitId(visitId);
            //This will grab the patient notes associated with the visit.
            viewModel.PatientNotesDetails = _listService.GetPatientNotesByVisitId(visitId);
            //This will grab the patient problems associated with the visit.
            viewModel.PatientProblemsDetails = _listService.GetPatientProblemsByVisitId(visitId);
            //This will grab the care plan associated with the visit.
            viewModel.CarePlansDetails = _listService.GetCarePlanByVisitId(visitId);
            //This will grab the med administration history associated with the visit.
            viewModel.MedAdministrationHistoriesDetails = _listService.GetMedHistoryByVisitId(visitId);
            //This will grab the vitals associated with the visit.
            viewModel.VitalsDetails = _listService.GetVitalsByVisitId(visitId);

            //This will add all of the data to a view bag that will be grabbed else where to display data correctly.
            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = mhn;

            return View(viewModel);
        }

        public IActionResult PatientVisits(int mhn)
        {
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = _listService.GetPatientByMHN(mhn);


            // New list to hold all the patients in the database.
            List<Visits> patientVisits = _listService.GetPatientVisitsByMHN(mhn);

            viewModel.Visits = patientVisits;
            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = mhn;

            return View(viewModel);
        }

        public IActionResult CreateVisitForm(int mhn)
        {
            // Needed to work with the patient banner properly.
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = _listService.GetPatientByMHN(mhn);

            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = mhn;

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult CreateVisitForm(Visits visit)
        {
            //returns the model if null because there were errors in validating it
            if (!ModelState.IsValid)
            {
                return View(visit);
            }

            return RedirectToAction("PatientVisits", new { mhn = visit.MHN });
        }
    }
}
