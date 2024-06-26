﻿using EHRApplication.Models;
using EHRApplication.Services;
using EHRApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Text.RegularExpressions;

namespace EHRApplication.Controllers
{
    public class MedicationsController : BaseController
    {
        private readonly ILogService _logService;
        private readonly string _connectionString;
        private readonly IListService _listService;

        public MedicationsController(ILogService logService, IListService listService, IConfiguration configuration)
            : base(logService, listService, configuration)
        {
            _logService = logService;
            this._connectionString = base._connectionString;
            _listService = listService;
    }

        /// <summary>
        /// Displays the view that shows all the medications in the database
        /// </summary>
        /// <returns></returns>
        public IActionResult AllMedications()
        {
            // New list to hold all the patients in the database.
            List<MedicationProfile> allMeds = new List<MedicationProfile>();

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                // Sql query.
                string sql = "SELECT medId, medName, description, route, activeStatus FROM [dbo].[MedicationProfile] ORDER BY CASE WHEN activeStatus = 1 THEN 0 ELSE 1 END, medName ASC";

                SqlCommand cmd = new SqlCommand(sql, connection);

                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        // Create a new patient object for each record.
                        MedicationProfile medication = new MedicationProfile();

                        // Populate the medication object with data from the database.
                        medication.medId = Convert.ToInt32(dataReader["medId"]);
                        medication.medName = Convert.ToString(dataReader["medName"]);
                        medication.description = Convert.ToString(dataReader["description"]);
                        medication.route = Convert.ToString(dataReader["route"]);
                        medication.activeStatus = Convert.ToBoolean(dataReader["activeStatus"]);

                        // Add the patient to the list
                        allMeds.Add(medication);
                    }
                }

                connection.Close();
            }

            // Return the view with the list of all the medications so we can display them.
            return View(allMeds);
        }

        public IActionResult PatientMedications(int mhn)
        {
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = _listService.GetPatientByMHN(mhn);

            // New list to hold all the patients in the database.
            List<PatientMedications> patientMeds = new List<PatientMedications>();

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                // Sql query.
                string sql = "SELECT * FROM [dbo].[PatientMedications] WHERE MHN = @mhn ORDER BY CASE WHEN activeStatus = 'Active' THEN 0 ELSE 1 END, datePrescribed ASC, endDate DESC";

                SqlCommand cmd = new SqlCommand(sql, connection);

                // Replace placeholder with paramater to avoid sql injection.
                cmd.Parameters.AddWithValue("@mhn", mhn);

                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        // Create a new patient object for each record.
                        PatientMedications medication = new PatientMedications();

                        // Populate the medication object with data from the database.
                        medication.patientMedId = Convert.ToInt32(dataReader["patientMedId"]);
                        medication.MHN = Convert.ToInt32(dataReader["MHN"]);
                        medication.medId = Convert.ToInt32(dataReader["medId"]);
                        medication.medProfile = _listService.GetMedicationProfileByMedId(medication.medId);
                        medication.category = Convert.ToString(dataReader["category"]);
                        medication.activeStatus = Convert.ToString(dataReader["activeStatus"]);
                        medication.prescriptionInstructions = Convert.ToString(dataReader["prescriptionInstructions"]);
                        medication.dosage = Convert.ToString(dataReader["dosage"]);
                        medication.route = Convert.ToString(dataReader["route"]);
                        medication.prescribedBy = Convert.ToInt32(dataReader["prescribedBy"]);
                        //Gets the provider for this patient using the primary physician number that links to the providers table
                        medication.providers = _listService.GetProvidersByProviderId(medication.prescribedBy);
                        medication.datePrescribed = Convert.ToDateTime(dataReader["datePrescribed"]);
                        medication.endDate = Convert.ToDateTime(dataReader["endDate"]);

                            // Add the patient to the list
                        patientMeds.Add(medication);
                    }
                }

                viewModel.PatientMedications = patientMeds;
                ViewBag.Patient = viewModel.PatientDemographic;
                ViewBag.MHN = mhn;
                connection.Close();
            }
            return View(viewModel);
        }

        public IActionResult MedicationOrders(int mhn)
        {
            //Creates a new instance of the viewModel
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = _listService.GetPatientByMHN(mhn);

            // New list to hold all the patients in the database.
            List<MedOrders> medOrdersList = new List<MedOrders>();

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                // Sql query to get the patient with the passed in mhn.
                string sql = "SELECT * FROM [dbo].[MedOrders] WHERE MHN = @mhn ORDER BY orderDate DESC, orderTime DESC";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@mhn", mhn);

                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        //Creating a new medorders instance
                        MedOrders medOrders = new MedOrders();

                        medOrders.orderId = Convert.ToInt32(dataReader["orderId"]);
                        medOrders.MHN = Convert.ToInt32(dataReader["MHN"]);
                        medOrders.patients = _listService.GetPatientByMHN(medOrders.MHN);

                        medOrders.visitId = Convert.ToInt32(dataReader["visitId"]);
                        medOrders.visits = _listService.GetVisitByVisitId(medOrders.visitId);

                        medOrders.medId = Convert.ToInt32(dataReader["medId"]);
                        medOrders.medProfile = _listService.GetMedicationProfileByMedId(medOrders.medId);

                        medOrders.frequency = Convert.ToString(dataReader["frequency"]);
                        medOrders.fulfillmentStatus = Convert.ToString(dataReader["fulfillmentStatus"]);
                        DateTime date = DateTime.Parse(dataReader["orderDate"].ToString());
                        medOrders.orderDate = new DateOnly(date.Year, date.Month, date.Day);
                        medOrders.orderTime = TimeOnly.Parse(dataReader["orderTime"].ToString());
                        medOrders.orderedBy = Convert.ToInt32(dataReader["orderedBy"]);
                        medOrders.providers = _listService.GetProvidersByProviderId(medOrders.orderedBy);

                        medOrdersList.Add(medOrders);
                    }
                }
                connection.Close();
                viewModel.MedOrders = medOrdersList;
                ViewBag.Patient = viewModel.PatientDemographic;
                ViewBag.MHN = mhn;
            }
            return View(viewModel);
        }
               

        public IActionResult MedAdministrationHistory(int mhn)
        {
            //This will set the banner up and the view model so we can view everything
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = _listService.GetPatientByMHN(mhn);

            //Calls the list service to get all of the med history associated to the passed in mhn number.
            List<MedAdministrationHistory> patientHistory = _listService.GetMedAdministrationHistoryByMHN(mhn);

            //This will add the patient history to the view model so it can be displayed along with the banner at the same time.
            viewModel.MedAdministrationHistories = patientHistory;

            //This will add all of the data to a view bag the will be grabbed else where to display data correctly.
            //ViewBag.PatientMedHistory = viewModel.MedAdministrationHistories;
            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = mhn;

            return View(viewModel);
        }

        public IActionResult PatientLabResults(int mhn)
        {
            //This will set the banner up and the view model so we can view everything
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = _listService.GetPatientByMHN(mhn);

            //Calls the list service to get all of the Lab Results associated to the passed in mhn number.
            List<LabResults> patientHistory = _listService.GetPatientsLabResultsByMHN(mhn);

            //This will add the patient Lab Results to the view model so it can be displayed along with the banner at the same time.
            viewModel.LabResults = patientHistory;

            //This will add all of the data to a view bag the will be grabbed else where to display data correctly.
            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = mhn;

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CreatePatientMedForm(int mhn)
        {
            // Needed to work with the patient banner properly.
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = _listService.GetPatientByMHN(mhn);

            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = mhn;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreatePatientMedForm(PatientMedications medication)
        {
            if (medication.prescriptionInstructions.IsNullOrEmpty())
            {
                ModelState.AddModelError("PatientMedication.prescriptionInstructions", "Please enter prescription instructions.");
            }
            if (medication.dosage.IsNullOrEmpty())
            {
                ModelState.AddModelError("PatientMedication.dosage", "Please enter prescribed dosage.");
            }
            if (medication.route.IsNullOrEmpty())
            {
                ModelState.AddModelError("PatientMedication.route", "Please enter medication route.");
            }
            if (medication.category.IsNullOrEmpty())
            {
                ModelState.AddModelError("PatientMedication.category", "Please enter something for category.");
            }else if (Regex.IsMatch(medication.category, @"\d"))
            {
                ModelState.AddModelError("PatientMedication.category", "Please enter only letters for category.");
            }
            if (medication.medId == 0)
            {
                ModelState.AddModelError("PatientMedication.medId", "Please select a medication.");
            }
            if (medication.prescribedBy == 0)
            {
                ModelState.AddModelError("PatientMedication.prescribedBy", "Please select a provider.");
            }
            // Validate startDate
            if (medication.datePrescribed == null)
            {
                ModelState.AddModelError("PatientMedication.datePrescribed", "Please enter start date.");
            }
            else
            {
                DateTime startDateTime = medication.datePrescribed;

                // Check if startDate is not set 30 days prior to the current date
                DateTime currentDate = DateTime.Now;
                if ((startDateTime - currentDate).Days < -730)
                {
                    ModelState.AddModelError("PatientMedication.datePrescribed", "Start date should not be set two years prior to the current date.");
                }
            }

            // Validate endDate
            if (medication.endDate == null)
            {
                ModelState.AddModelError("PatientMedication.endDate", "Please enter end date.");
            }
            else
            {
                DateTime endDateTime = medication.endDate;

                // Check if endDate is a future date
                DateTime currentDate = DateTime.Now;

                // Check if endDate is after startDate
                DateTime startDateTime = medication.datePrescribed;
                if (endDateTime <= startDateTime)
                {
                    ModelState.AddModelError("PatientMedication.endDate", "End date should be after start date.");
                }

                // Check if startDate and endDate are not the same date
                if (startDateTime.Date == endDateTime.Date)
                {
                    ModelState.AddModelError("PatientMedication.endDate", "End date should not be the same as start date.");
                }
            }

            //returns the model if null because there were errors in validating it
            if (!ModelState.IsValid)
            {
                // Needed to work with the patient banner properly.
                PortalViewModel viewModel = new PortalViewModel();
                viewModel.PatientDemographic = _listService.GetPatientByMHN(medication.MHN);
                viewModel.PatientMedication = medication;
                ViewBag.Patient = viewModel.PatientDemographic;
                ViewBag.MHN = medication.MHN;

                return View(viewModel);
            }
            else if (medication.MHN != 0)
            {
                //go to the void list service that will input the data into the database.
                _listService.InsertIntoPatientMed(medication);
            }  

            return RedirectToAction("PatientMedications", new { mhn = medication.MHN });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult EditPatientMedForm(int patientMedId)
        {
            // Needed to work with the patient banner properly.
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientMedication = _listService.GetPatientsMedByPatientMedId(patientMedId);
            viewModel.PatientDemographic = _listService.GetPatientByMHN(viewModel.PatientMedication.MHN);

            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = viewModel.PatientMedication.MHN;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult EditPatientMedForm(PatientMedications medication)
        {
            if (medication.prescriptionInstructions.IsNullOrEmpty())
            {
                ModelState.AddModelError("PatientMedication.prescriptionInstructions", "Please enter prescription instructions.");
            }
            if (medication.dosage.IsNullOrEmpty())
            {
                ModelState.AddModelError("PatientMedication.dosage", "Please enter prescribed dosage.");
            }
            if (medication.route.IsNullOrEmpty())
            {
                ModelState.AddModelError("PatientMedication.route", "Please enter medication route.");
            }
            if (medication.category.IsNullOrEmpty())
            {
                ModelState.AddModelError("PatientMedication.category", "Please enter something for category.");
            }
            else if (Regex.IsMatch(medication.category, @"\d"))
            {
                ModelState.AddModelError("PatientMedication.category", "Please enter only letters for category.");
            }
            if (medication.medId == 0)
            {
                ModelState.AddModelError("PatientMedication.medId", "Please select a medication.");
            }
            if (medication.prescribedBy == 0)
            {
                ModelState.AddModelError("PatientMedication.prescribedBy", "Please select a provider.");
            }
            // Validate startDate
            if (medication.datePrescribed == null)
            {
                ModelState.AddModelError("PatientMedication.datePrescribed", "Please enter start date.");
            }
            else
            {
                DateTime startDateTime = medication.datePrescribed;

                // Check if startDate is not set 30 days prior to the current date
                DateTime currentDate = DateTime.Now;
                if ((startDateTime - currentDate).Days < -730)
                {
                    ModelState.AddModelError("PatientMedication.datePrescribed", "Start date should not be set more than two years prior to the current date.");
                }
            }

            // Validate endDate
            if (medication.endDate == null)
            {
                ModelState.AddModelError("PatientMedication.endDate", "Please enter end date.");
            }
            else
            {
                DateTime endDateTime = medication.endDate;

                // Check if endDate is a future date
                DateTime currentDate = DateTime.Now;

                // Check if endDate is after startDate
                DateTime startDateTime = medication.datePrescribed;
                if (endDateTime <= startDateTime)
                {
                    ModelState.AddModelError("PatientMedication.endDate", "End date should be after start date.");
                }

                // Check if startDate and endDate are not the same date
                if (startDateTime.Date == endDateTime.Date)
                {
                    ModelState.AddModelError("PatientMedication.endDate", "End date should not be the same as start date.");
                }
            }

            //returns the model if null because there were errors in validating it
            if (!ModelState.IsValid)
            {
                // Needed to work with the patient banner properly.
                PortalViewModel viewModel = new PortalViewModel();
                viewModel.PatientDemographic = _listService.GetPatientByMHN(medication.MHN);
                viewModel.PatientMedication = medication;
                ViewBag.Patient = viewModel.PatientDemographic;
                ViewBag.MHN = medication.MHN;

                return View(viewModel);
            }
            else if (medication.MHN != 0)
            {
                //go to the void list service that will input the data into the database.
                _listService.UpdatePatientMed(medication);
            }

            return RedirectToAction("PatientMedications", new { mhn = medication.MHN });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateActiveStatusForMedProfile(int medId, bool activeStatus)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                // Sql query.
                string sql = "UPDATE [dbo].[MedicationProfile] SET activeStatus = @active WHERE medId = @medId";

                SqlCommand cmd = new SqlCommand(sql, connection);

                // Replace placeholder with parameter to avoid SQL injection.
                cmd.Parameters.AddWithValue("@medId", medId);
                cmd.Parameters.AddWithValue("@active", activeStatus);

                // Execute the SQL command.
                int rowsAffected = cmd.ExecuteNonQuery();

                connection.Close();

                // Check if any rows were affected.
                if (rowsAffected > 0)
                {
                    // Successfully updated.
                    return Ok("Successfully updated.");
                }
                else
                {
                    // No rows were affected, return an error message.
                    return BadRequest("Error, please try again.");
                }
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CreateMedication()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateMedication(MedicationProfile medProfile)
        {
            if (medProfile.route == "")
            {
                ModelState.AddModelError("route", "Please select a route.");
            }
            if (!Regex.IsMatch(medProfile.description, @"^[a-zA-Z0-9\s.,'""!?()\-]*$"))
            {
                ModelState.AddModelError("MedicationProfile.description", "Description must only contain letters and spaces.");
            }
            if (!ModelState.IsValid)
            {
                return View(medProfile);
            }
            else
            {
                _listService.InsertIntoMedProfile(medProfile);
            }
            return RedirectToAction("AllMedications");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult EditMedication(int medId)
        {

            MedicationProfile medProfile= _listService.GetMedicationProfileByMedId(medId);

            return View(medProfile);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult EditMedication(MedicationProfile medProfile)
        {
            if (medProfile.route == "")
            {
                ModelState.AddModelError("route", "Please select a route.");
            }
            if (!Regex.IsMatch(medProfile.description, @"^[a-zA-Z0-9\s.,'""!?()\-]*$"))
            {
                ModelState.AddModelError("MedicationProfile.description", "Description must only contain letters and spaces.");
            }
            if (!ModelState.IsValid)
            {
                return View(medProfile);
            }
            else
            {
                _listService.UpdateMedProfile(medProfile);
            }
            return RedirectToAction("AllMedications");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CreateMedicationOrder(int mhn)
        {
            // Needed to work with the patient banner properly.
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = _listService.GetPatientByMHN(mhn);

            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = mhn;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateMedicationOrder(MedOrders medOrder)
        {
            //Vailidatoin that can't be done in the model
            if (medOrder.visitId == 0)
            {
                ModelState.AddModelError("MedOrdersDetails.visitId", "Please select a visit.");
            }
            if (medOrder.fulfillmentStatus.IsNullOrEmpty())
            {
                ModelState.AddModelError("MedOrdersDetails.fulfillmentStatus", "Please select a status.");
            }
            if (medOrder.medId == 0)
            {
                ModelState.AddModelError("MedOrdersDetails.medId", "Please select a medication.");
            }
            if (medOrder.orderedBy == 0)
            {
                ModelState.AddModelError("MedOrdersDetails.orderedBy", "Please select a provider.");
            }
            // Testing to see if the date of birth entered was a future date or not
            if (medOrder.orderDate > DateOnly.FromDateTime(DateTime.Now))
            {
                // Adding an error to the DOB model to display an error.
                ModelState.AddModelError("MedOrdersDetails.orderDate", "Date cannot be in the future.");
            }
            if (medOrder.frequency.IsNullOrEmpty())
            {
                ModelState.AddModelError("MedOrdersDetails.frequency", "Please enter a value for frequency.");
            }

            //returns the model if null because there were errors in validating it
            if (!ModelState.IsValid)
            {
                // Needed to work with the patient banner properly.
                PortalViewModel viewModel = new PortalViewModel();
                viewModel.PatientDemographic = _listService.GetPatientByMHN(medOrder.MHN);
                viewModel.MedOrdersDetails = medOrder;
                ViewBag.Patient = viewModel.PatientDemographic;
                ViewBag.MHN = medOrder.MHN;

                return View(viewModel);
            }
            else if (medOrder.MHN != 0)
            {
                //go to the void list service that will input the data into the database.
                _listService.InsertIntoMedOrder(medOrder);
            }

            return RedirectToAction("MedicationOrders", new { mhn = medOrder.MHN });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult EditMedicationOrder(int orderId)
        {
            // Needed to work with the patient banner properly.
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.MedOrdersDetails = _listService.GetMedOrderByOrderId(orderId);
            viewModel.PatientDemographic = _listService.GetPatientByMHN(viewModel.MedOrdersDetails.MHN);

            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = viewModel.MedOrdersDetails.MHN;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult EditMedicationOrder(MedOrders medOrder)
        {
            //Vailidatoin that can't be done in the model
            if (medOrder.visitId == 0)
            {
                ModelState.AddModelError("MedOrdersDetails.visitId", "Please select a visit.");
            }
            if (medOrder.fulfillmentStatus.IsNullOrEmpty())
            {
                ModelState.AddModelError("MedOrdersDetails.fulfillmentStatus", "Please select a status.");
            }
            if (medOrder.medId == 0)
            {
                ModelState.AddModelError("MedOrdersDetails.medId", "Please select a medication.");
            }
            if (medOrder.orderedBy == 0)
            {
                ModelState.AddModelError("MedOrdersDetails.orderedBy", "Please select a provider.");
            }
            // Testing to see if the date of birth entered was a future date or not
            if (medOrder.orderDate > DateOnly.FromDateTime(DateTime.Now))
            {
                // Adding an error to the DOB model to display an error.
                ModelState.AddModelError("MedOrdersDetails.orderDate", "Date cannot be in the future.");
            }
            if (medOrder.frequency.IsNullOrEmpty())
            {
                ModelState.AddModelError("MedOrdersDetails.frequency", "Please enter a value for frequency.");
            }

            //returns the model if null because there were errors in validating it
            if (!ModelState.IsValid)
            {
                // Needed to work with the patient banner properly.
                PortalViewModel viewModel = new PortalViewModel();
                viewModel.PatientDemographic = _listService.GetPatientByMHN(medOrder.MHN);
                viewModel.MedOrdersDetails = medOrder;
                ViewBag.Patient = viewModel.PatientDemographic;
                ViewBag.MHN = medOrder.MHN;

                return View(viewModel);
            }
            else if (medOrder.MHN != 0)
            {
                //go to the void list service that will input the data into the database.
                _listService.UpdateMedOrder(medOrder);
            }

            return RedirectToAction("MedicationOrders", new { mhn = medOrder.MHN });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CreateMedAdministrationHistory(int mhn)
        {
            // Needed to work with the patient banner properly.
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = _listService.GetPatientByMHN(mhn);

            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = mhn;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateMedAdministrationHistory(MedAdministrationHistory medHistory)
        {
            //Vailidatoin that can't be done in the model
            if (medHistory.visitsId == 0)
            {
                ModelState.AddModelError("MedAdministrationHistoriesDetails.visitsId", "Please select a visit.");
            }
            if (medHistory.administeredBy == 0)
            {
                ModelState.AddModelError("MedAdministrationHistoriesDetails.administeredBy", "Please select a visit.");
            }
            if (medHistory.medId == 0)
            {
                ModelState.AddModelError("MedAdministrationHistoriesDetails.medId", "Please select a medication.");
            }
            if (medHistory.category.IsNullOrEmpty())
            {
                ModelState.AddModelError("MedAdministrationHistoriesDetails.category", "Please select a provider.");
            }
            // Testing to see if the date of birth entered was a future date or not
            if (medHistory.dateGiven > DateOnly.FromDateTime(DateTime.Now))
            {
                // Adding an error to the DOB model to display an error.
                ModelState.AddModelError("MedAdministrationHistoriesDetails.dateGiven", "Date cannot be in the future.");
            }
            if (medHistory.frequency.IsNullOrEmpty())
            {
                ModelState.AddModelError("MedAdministrationHistoriesDetails.frequency", "Please enter a value for frequency.");
            }
            if (medHistory.status.IsNullOrEmpty())
            {
                ModelState.AddModelError("MedAdministrationHistoriesDetails.status", "Please select a status.");
            }

            //returns the model if null because there were errors in validating it
            if (!ModelState.IsValid)
            {
                // Needed to work with the patient banner properly.
                PortalViewModel viewModel = new PortalViewModel();
                viewModel.PatientDemographic = _listService.GetPatientByMHN(medHistory.MHN);
                viewModel.MedAdministrationHistoriesDetails = medHistory;
                ViewBag.Patient = viewModel.PatientDemographic;
                ViewBag.MHN = medHistory.MHN;

                return View(viewModel);
            }
            else if (medHistory.MHN != 0)
            {
                //go to the void list service that will input the data into the database.
                _listService.InsertIntoAdministrationHistory(medHistory);
            }

            return RedirectToAction("MedAdministrationHistory", new { mhn = medHistory.MHN });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult EditMedAdministrationHistory(int administrationId)
        {
            // Needed to work with the patient banner properly.
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.MedAdministrationHistoriesDetails = _listService.GetMedAdministrationHistoryByAdminId(administrationId);
            viewModel.PatientDemographic = _listService.GetPatientByMHN(viewModel.MedAdministrationHistoriesDetails.MHN);

            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = viewModel.MedAdministrationHistoriesDetails.MHN;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult EditMedAdministrationHistory(MedAdministrationHistory medHistory)
        {
            //Vailidatoin that can't be done in the model
            if (medHistory.visitsId == 0)
            {
                ModelState.AddModelError("MedAdministrationHistoriesDetails.visitsId", "Please select a visit.");
            }
            if (medHistory.administeredBy == 0)
            {
                ModelState.AddModelError("MedAdministrationHistoriesDetails.administeredBy", "Please select a visit.");
            }
            if (medHistory.medId == 0)
            {
                ModelState.AddModelError("MedAdministrationHistoriesDetails.medId", "Please select a medication.");
            }
            if (medHistory.category.IsNullOrEmpty())
            {
                ModelState.AddModelError("MedAdministrationHistoriesDetails.category", "Please select a provider.");
            }
            // Testing to see if the date of birth entered was a future date or not
            if (medHistory.dateGiven > DateOnly.FromDateTime(DateTime.Now))
            {
                // Adding an error to the DOB model to display an error.
                ModelState.AddModelError("MedAdministrationHistoriesDetails.dateGiven", "Date cannot be in the future.");
            }
            if (medHistory.frequency.IsNullOrEmpty())
            {
                ModelState.AddModelError("MedAdministrationHistoriesDetails.frequency", "Please enter a value for frequency.");
            }
            if (medHistory.status.IsNullOrEmpty())
            {
                ModelState.AddModelError("MedAdministrationHistoriesDetails.status", "Please select a status.");
            }

            //returns the model if null because there were errors in validating it
            if (!ModelState.IsValid)
            {
                // Needed to work with the patient banner properly.
                PortalViewModel viewModel = new PortalViewModel();
                viewModel.PatientDemographic = _listService.GetPatientByMHN(medHistory.MHN);
                viewModel.MedAdministrationHistoriesDetails = medHistory;
                ViewBag.Patient = viewModel.PatientDemographic;
                ViewBag.MHN = medHistory.MHN;

                return View(viewModel);
            }
            else if (medHistory.MHN != 0)
            {
                //go to the void list service that will input the data into the database.
                _listService.UpdateAdministrationHistory(medHistory);
            }

            return RedirectToAction("MedADministrationHistory", new { mhn = medHistory.MHN });
        }

        [HttpPost]
        [Route("Medication/DeletePatientMedication")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeletePatientMedication(int patientMedId)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                string sql = "DELETE FROM [PatientMedications] WHERE patientMedId = @patientMedId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@patientMedId", SqlDbType.Int).Value = patientMedId;

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        connection.Close();

                        if (rowsAffected <= 0)
                        {
                            throw new Exception(patientMedId + " not found.");
                        }
                        else
                        {
                            return Ok("Successfully deleted medication.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.ToString());
                        return BadRequest("Failed to delete medication.");
                    }
                }
            }
        }

        [HttpPost]
        [Route("Medication/DeleteMedAdminHistory")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteMedAdminHistory(int administrationId)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                string sql = "DELETE FROM [MedAdministrationHistory] WHERE administrationId = @administrationId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@administrationId", SqlDbType.Int).Value = administrationId;

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        connection.Close();

                        if (rowsAffected <= 0)
                        {
                            throw new Exception(administrationId + " not found.");
                        }
                        else
                        {
                            return Ok("Successfully deleted administration history.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.ToString());
                        return BadRequest("Failed to delete administration history.");
                    }
                }
            }
        }

        [HttpPost]
        [Route("Medication/DeleteMedOrder")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteMedOrder(int orderId)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                string sql = "DELETE FROM [MedOrders] WHERE orderId = @orderId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@orderId", SqlDbType.Int).Value = orderId;

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        connection.Close();

                        if (rowsAffected <= 0)
                        {
                            throw new Exception(orderId + " not found.");
                        }
                        else
                        {
                            return Ok("Successfully deleted medication order.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.ToString());
                        return BadRequest("Failed to delete medication order.");
                    }
                }
            }
        }
    }
}