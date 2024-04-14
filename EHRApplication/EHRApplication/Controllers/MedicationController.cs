﻿using EHRApplication.Models;
using EHRApplication.Services;
using EHRApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

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
                string sql = "SELECT * FROM [dbo].[MedicationProfile] ORDER BY medName ASC";

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
                string sql = "SELECT * FROM [dbo].[PatientMedications] WHERE MHN = @mhn ORDER BY medId ASC";

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

        public IActionResult PatientMedHistory(int mhn)
        {
            //This will set the banner up and the view model so we can view everything
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = GetPatientByMHN(mhn);

            //Calls the list service to get all of the med history associated to the passed in mhn number.
            List<MedAdministrationHistory> patientHistory = _listService.GetPatientsMedHistoryByMHN(mhn);

            //This will add the patient history to the view model so it can be displayed along with the banner at the same time.
            viewModel.MedAdministrationHistories = patientHistory;

            //This will add all of the data to a view bag the will be grabbed else where to display data correctly.
            ViewBag.PatientMedHistory = viewModel.MedAdministrationHistories;
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

            return RedirectToAction("PatientMedHistory", new { mhn = medication.MHN });
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
    }
}