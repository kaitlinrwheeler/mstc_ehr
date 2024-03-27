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
            Visits visit = _listService.GetVisitByVisitId(visitId);

            //This will grab the lab results associated with the visit.
            LabResults labResults = _listService.GetLabResultsByVisitId(visitId);
            //This will grab the lab order associated with the visit.
            LabOrders labOrders = _listService.GetLabOrdersByVisitId(visitId);
            //This will grab the med order associated with the visit.
            MedOrders medOrders = _listService.GetMedOrdersByVisitId(visitId);
            //This will grab the patient notes associated with the visit.
            PatientNotes patientNotes = _listService.GetPatientNotesByVisitId(visitId);
            //This will grab the patient problems associated with the visit.
            PatientProblems patientProblems = _listService.GetPatientProblemsByVisitId(visitId);
            //This will grab the care plan associated with the visit.
            CarePlan carePlan = _listService.GetCarePlanByVisitId(visitId);
            //This will grab the med administration history associated with the visit.
            MedAdministrationHistory medHistory = _listService.GetMedHistoryByVisitId(visitId);

            viewModel.VisitDetails = visit;/*
            viewModel.LabResults = labResults;
            viewModel.LabOrders = labOrders;
            viewModel.MedOrders = medOrders;
            viewModel.PatientNotes = patientNotes;
            viewModel.PatientProblems = patientProblems;
            viewModel.CarePlans = carePlan;
            viewModel.MedAdministrationHistories = medHistory;*/

            //This will add all of the data to a view bag that will be grabbed else where to display data correctly.
            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = mhn;

            return View(viewModel);
        }
    }
}
