using EHRApplication.Models;
using EHRApplication.Services;
using EHRApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
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
            viewModel.PatientDemographic = GetPatientByMHN(mhn);

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
            //Creates a new instance of hte viewModel
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = GetPatientByMHN(mhn);

            // New list to hold all the patients in the database.
            List<MedOrders> medOrdersList = new List<MedOrders>();

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                // Sql query to get the patient with the passed in mhn.
                string sql = "SELECT orderId, MHN, visitId, medId, frequency, fulfillmentStatus, orderDate, orderTime, orderedBy " +
                    "FROM [dbo].[MedOrders] ORDER BY orderDate DESC, orderTime DESC";

                SqlCommand cmd = new SqlCommand(sql, connection);


                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        //Creating a new medorders instance
                        MedOrders medOrders = new MedOrders();

                        medOrders.orderId = Convert.ToInt32(dataReader["orderId"]);
                        medOrders.MHN = Convert.ToInt32(dataReader["MHN"]);
                        medOrders.patients = GetPatientByMHN(medOrders.MHN);

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

        private PatientDemographic GetPatientByMHN(int mhn)
        {
            //Creating a new patientDemographic instance
            PatientDemographic patientDemographic = new PatientDemographic();

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                // Sql query to get the patient with the passed in mhn.
                string sql = "SELECT * FROM [dbo].[PatientDemographic] WHERE MHN = @mhn";

                SqlCommand cmd = new SqlCommand(sql, connection);

                // Replace placeholder with paramater to avoid sql injection.
                cmd.Parameters.AddWithValue("@mhn", mhn);

                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        //Assign properties for the patient demographic from the database
                        patientDemographic.MHN = Convert.ToInt32(dataReader["MHN"]);
                        patientDemographic.firstName = Convert.ToString(dataReader["firstName"]);
                        patientDemographic.middleName = Convert.ToString(dataReader["middleName"]);
                        patientDemographic.lastName = Convert.ToString(dataReader["lastName"]);
                        patientDemographic.suffix = Convert.ToString(dataReader["suffix"]);
                        patientDemographic.preferredPronouns = Convert.ToString(dataReader["preferredPronouns"]);
                        //This is grabbing the date of birth from the database and converting it to date only. Somehow even though it is 
                        //Saved to the database as only a date it does not read as just a date so this converts it to dateOnly.
                        DateTime dateTime = DateTime.Parse(dataReader["DOB"].ToString());
                        patientDemographic.DOB = new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
                        patientDemographic.gender = Convert.ToString(dataReader["gender"]);
                        patientDemographic.preferredLanguage = Convert.ToString(dataReader["preferredLanguage"]);
                        patientDemographic.ethnicity = Convert.ToString(dataReader["ethnicity"]);
                        patientDemographic.race = Convert.ToString(dataReader["race"]);
                        patientDemographic.religion = Convert.ToString(dataReader["religion"]);
                        patientDemographic.primaryPhysician = Convert.ToInt32(dataReader["primaryPhysician"]);
                        //Gets the provider for this patient using the primary physician number that links to the providers table
                        patientDemographic.providers = _listService.GetProvidersByProviderId(patientDemographic.primaryPhysician);
                        patientDemographic.legalGuardian1 = Convert.ToString(dataReader["legalGuardian1"]);
                        patientDemographic.legalGuardian2 = Convert.ToString(dataReader["legalGuardian2"]);
                        patientDemographic.previousName = Convert.ToString(dataReader["previousName"]);
                        //Gets the contact info for this patient using the MHN that links to the contact info table
                        patientDemographic.genderAssignedAtBirth = Convert.ToString(dataReader["genderAssignedAtBirth"]);
                        patientDemographic.ContactId = _listService.GetContactByMHN(patientDemographic.MHN);
                        patientDemographic.HasAlerts = Convert.ToBoolean(dataReader["HasAlerts"]);
                    }
                }

                connection.Close();
            }

            return patientDemographic;
        }

        public IActionResult MedAdministrationHistory(int mhn)
        {
            //This will set the banner up and the view model so we can view everything
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = GetPatientByMHN(mhn);

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
            viewModel.PatientDemographic = GetPatientByMHN(mhn);

            //Calls the list service to get all of the Lab Results associated to the passed in mhn number.
            List<LabResults> patientHistory = _listService.GetPatientsLabResultsByMHN(mhn);

            //This will add the patient Lab Results to the view model so it can be displayed along with the banner at the same time.
            viewModel.LabResults = patientHistory;

            //This will add all of the data to a view bag the will be grabbed else where to display data correctly.
            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = mhn;

            return View(viewModel);
        }

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

        public IActionResult CreateMedication()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateMedication(MedicationProfile medProfile)
        {
            if (medProfile.route == "")
            {
                ModelState.AddModelError("route", "Please select a route.");
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

        public IActionResult EditMedication(int medId)
        {

            MedicationProfile medProfile= _listService.GetMedicationProfileByMedId(medId);

            return View(medProfile);
        }

        [HttpPost]
        public IActionResult EditMedication(MedicationProfile medProfile)
        {
            if (medProfile.route == "")
            {
                ModelState.AddModelError("route", "Please select a route.");
            }
            if(!ModelState.IsValid)
            {
                return View(medProfile);
            }
            else
            {
                _listService.UpdateMedProfile(medProfile);
            }
            return RedirectToAction("AllMedications");
        }

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
            else if (!Regex.Match(medOrder.frequency, @"^[a-zA-Z\s'\/\-]+$").Success)
            {
                ModelState.AddModelError("MedOrdersDetails.frequency", "Please only enter letters.");
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
            else if (!Regex.Match(medOrder.frequency, @"^[a-zA-Z\s'\/\-]+$").Success)
            {
                ModelState.AddModelError("MedOrdersDetails.frequency", "Please only enter letters.");
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
            else if (!Regex.Match(medHistory.status, @"^[a-zA-Z\s'\/\-]+$").Success)
            {
                ModelState.AddModelError("MedAdministrationHistoriesDetails.status", "Please only enter letters.");
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
            else if (!Regex.Match(medHistory.status, @"^[a-zA-Z\s'\/\-]+$").Success)
            {
                ModelState.AddModelError("MedAdministrationHistoriesDetails.status", "Please only enter letters.");
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
    }
}