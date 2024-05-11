using EHRApplication.Models;
using EHRApplication.Services;
using EHRApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = _listService.GetPatientByMHN(visit.MHN);
            viewModel.Visit = visit;
            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = visit.MHN;

            if (visit.providerId == 0)
            {
                ModelState.AddModelError("Visit.providerId", "Please select a provider.");
            }
            // Testing to see if the date of birth entered was a future date or not
            if (visit.date >= DateOnly.FromDateTime(DateTime.Now))
            {
                // Adding an error to the DOB model to display an error.
                ModelState.AddModelError("Visit.date", "Date cannot be in the future.");
                return View(viewModel);
            }
            // Testing to see if the date of birth entered is before 1920 or not
            if (visit.date < DateOnly.FromDateTime(new DateTime(1900, 1, 1)))
            {
                // Adding an error to the date model to display an error.
                ModelState.AddModelError("Visit.date", "Date cannot be before 1920.");
                return View(viewModel);
            }
            //returns the model if null because there were errors in validating it
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            else if (visit.MHN != 0)
            {
                //go to the void list service that will input the data into the database.
                _listService.InsertIntoVisits(visit);
            }

            return RedirectToAction("PatientVisits", new { mhn = visit.MHN });
        }

        public IActionResult EditVisitForm(int visitId)
        {
            // Needed to work with the patient banner properly.
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.Visit = _listService.GetVisitByVisitId(visitId);
            viewModel.PatientDemographic = _listService.GetPatientByMHN(viewModel.Visit.MHN);

            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = viewModel.Visit.MHN;

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult EditVisitForm(Visits visit)
        {
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = _listService.GetPatientByMHN(visit.MHN);
            viewModel.Visit = visit;
            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = visit.MHN;

            if (visit.providerId == 0)
            {
                ModelState.AddModelError("Visit.providerId", "Please select a provider.");
            }
            // Testing to see if the date of birth entered was a future date or not
            if (visit.date >= DateOnly.FromDateTime(DateTime.Now))
            {
                // Adding an error to the DOB model to display an error.
                ModelState.AddModelError("Visit.date", "Date cannot be in the future.");
                return View(viewModel);
            }
            // Testing to see if the date of birth entered is before 1920 or not
            if (visit.date < DateOnly.FromDateTime(new DateTime(1900, 1, 1)))
            {
                // Adding an error to the date model to display an error.
                ModelState.AddModelError("Visit.date", "Date cannot be before 1920.");
                return View(viewModel);
            }
            //returns the model if null because there were errors in validating it
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            else if (visit.MHN != 0)
            {
                //go to the void list service that will update the data into the database.
                _listService.UpdateVisits(visit);
            }

            return RedirectToAction("PatientVisits", new { mhn = visit.MHN });
        }

        [HttpPost]
        public IActionResult DeleteVisit(int visitId)
        {

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                // Begin a transaction
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // SQL query to delete the visit and related records
                    string deleteVisitSql = "DELETE FROM [dbo].[Visits] WHERE visitsId = @visitId";

                    // All records related to the visit in some way or another that also must be deleted before we can.
                    string deleteRelatedRecordsSql = @"
                        DELETE FROM [dbo].[LabResults] WHERE visitsId = @visitId;
                        DELETE FROM [dbo].[Vitals] WHERE visitId = @visitId;
                        DELETE FROM [dbo].[LabOrders] WHERE visitsId = @visitId;
                        DELETE FROM [dbo].[MedOrders] WHERE visitId = @visitId;
                        DELETE FROM [dbo].[PatientNotes] WHERE visitsId = @visitId;
                        DELETE FROM [dbo].[PatientProblems] WHERE visitsId = @visitId;
                        DELETE FROM [dbo].[CarePlan] WHERE visitsId = @visitId;
                        DELETE FROM [dbo].[MedAdministrationHistory] WHERE visitsId = @visitId;
                    ";


                    // First, delete related records
                    using (SqlCommand cmd = new SqlCommand(deleteRelatedRecordsSql, connection, transaction))
                    {
                        // Set parameters if needed
                        cmd.Parameters.AddWithValue("@visitId", visitId);

                        int relatedRowsAffected = cmd.ExecuteNonQuery();
                    }

                    // Now, that everything related to the patient is deleted, delete the patient.
                    using (SqlCommand cmd = new SqlCommand(deleteVisitSql, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@visitId", visitId);

                        int patientRowsAffected = cmd.ExecuteNonQuery();

                        // Check if the patient was deleted
                        if (patientRowsAffected <= 0)
                        {
                            throw new Exception("Visit with visitId " + visitId + " not found.");
                        }
                    }

                    // If everything went well, commit the transaction
                    transaction.Commit();

                    return Ok("Successfully deleted.");
                }
                catch (Exception ex)
                {
                    // Something went wrong, rollback the transaction
                    transaction.Rollback();

                    // Log the exception if needed
                    Console.WriteLine("Error: " + ex.Message);

                    // Return an error response
                    return BadRequest("Failed to delete visit.");
                }
            }
        }
    }
}
