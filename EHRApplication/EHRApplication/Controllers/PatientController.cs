﻿using EHRApplication.Models;
using EHRApplication.Services;
using EHRApplication.ViewModels;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Security.Cryptography.Pkcs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EHRApplication.Controllers
{
    public class PatientController : BaseController
    {
        private readonly ILogService _logService;
        private readonly string _connectionString;
        private readonly IListService _listService;

        public PatientController(ILogService logService, IListService listService, IConfiguration configuration)
            : base(logService, listService, configuration)
        {
            _logService = logService;
            this._connectionString = base._connectionString;
            _listService = listService;
    }

        public IActionResult AllPatients()
        {
            // New list to hold all the patients in the database.
            List<PatientDemographic> allPatients = new List<PatientDemographic>();

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                // Sql query.
                string sql = "SELECT MHN, firstName, lastName, DOB, gender, Active FROM [dbo].[PatientDemographic] ORDER BY MHN ASC";

                SqlCommand cmd = new SqlCommand(sql, connection);

                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        // Create a new patient object for each record.
                        PatientDemographic patient = new PatientDemographic();

                        // Populate the patient object with data from the database.
                        // Note: there are more fields in a patient demographic, but these are the only ones we are wanting to display.
                        // Change this if you want more of less fields.
                        patient.MHN = Convert.ToInt32(dataReader["MHN"]);
                        patient.firstName = Convert.ToString(dataReader["firstName"]);
                        patient.lastName = Convert.ToString(dataReader["lastName"]);
                        //This is grabbing the date from the database and converting it to date only. Somehow even though it is 
                        //Saved to the database as only a date it does not read as just a date so this converts it to dateOnly.
                        DateTime dateTime = DateTime.Parse(dataReader["DOB"].ToString());
                        patient.DOB = new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
                        patient.gender = Convert.ToString(dataReader["gender"]);
                        patient.Active = Convert.ToBoolean(dataReader["Active"]);

                        // Add the patient to the list
                        allPatients.Add(patient);
                    }
                }

                connection.Close();
            }

            // Return the view with the list of all the patients so we can display them.
            return View(allPatients);
        }


        public IActionResult Index(int mhn)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(PatientDemographic patient)
        {
            //Testing to see if the date of birth entered was a future date or not
            if (patient.DOB >= DateOnly.FromDateTime(DateTime.Now))
            {
                //adding an error to the DOB model to display an error.
                ModelState.AddModelError("DOB", "Date cannot be in the future.");
                return View(patient);
            }
            //returns the model if null because there were errors in validating it
            if (!ModelState.IsValid)
            {
                return View(patient);
            }

            // process file upload
            if (patient.patientImageFile != null && patient.patientImageFile.Length > 0)
            {
                // define permitted image file types
                var allowedFileTypes = new[] { ".jpg", ".png" };
                var extentions = Path.GetExtension(patient.patientImageFile.FileName).ToLowerInvariant();

                // validate file type
                if (!allowedFileTypes.Contains(extentions))
                {
                    ModelState.AddModelError("patientImage", "Invalid file type. Only image files ending in .jpg, or .png are permitted.");
                    return View(patient);
                }

                // create filepath to storage location
                var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                // check for existence of directory and create if it doesn't exist
                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory);
                }

                // create and assign new filename before storage
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(patient.patientImageFile.FileName);

                // create full filepath
                var filePath = Path.Combine(uploadDirectory, fileName);

                // save file to disk
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    patient.patientImageFile.CopyToAsync(stream);
                }

                // save filepath to pt image property
                patient.patientImage = fileName;

            }

            // process race selection
            if (patient.race == null && !string.IsNullOrEmpty(patient.OtherRace))
            {
                // if other race is chosen rather than offered selection
                // set race to 'other race' property
                patient.race = patient.OtherRace;
            }
            else
            {
                patient.race = string.Join(",", patient.raceList);
            }

            // process preferred pronouns selection
            if (patient.preferredPronouns == "Other" && !string.IsNullOrEmpty(patient.OtherPronouns))
            {
                // if other pronouns is chosen rather than offered selection
                // set pronouns to 'other preferred pronouns' property
                patient.preferredPronouns = patient.OtherPronouns;
            }

            // process gender selection
            if (patient.gender == "Other" && !string.IsNullOrEmpty(patient.OtherGender))
            {
                // if other gender is chosen rather than offered selection
                // set gender to 'other gender' property
                patient.gender = patient.OtherGender;
            }            

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                //SQL query that is going to insert the data that the user entered into the database table.
                string sql = "INSERT INTO [PatientDemographic] (firstName, middleName, lastName, suffix, preferredPronouns, DOB, gender, preferredLanguage, ethnicity, race, religion, primaryPhysician, legalGuardian1, legalGuardian2, previousName, genderAssignedAtBirth, patientImage) " +
                    "VALUES (@firstName, @middleName, @lastName, @suffix, @preferredPronouns, @DOB, @gender, @preferredLanguage, @ethnicity, @race, @religion, @primaryPhysician, @legalGuardian1, @legalGuardian2, @previousName, @genderAssignedAtBirth, @patientImage)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;

                    //The some of them test to see if the value if null or empty because they are optional on the form so if it is null or empty it will display NA otherwise will add the data enterd by the user.
                    //adding parameters
                    command.Parameters.Add("@firstName", SqlDbType.VarChar).Value = patient.firstName;
                    command.Parameters.Add("@middleName", SqlDbType.VarChar).Value = string.IsNullOrEmpty(patient.middleName) ? "" : patient.middleName;
                    command.Parameters.Add("@lastName", SqlDbType.VarChar).Value = patient.lastName;
                    command.Parameters.Add("@suffix", SqlDbType.VarChar).Value = string.IsNullOrEmpty(patient.suffix) ? "" : patient.suffix;
                    command.Parameters.Add("@preferredPronouns", SqlDbType.VarChar).Value = string.IsNullOrEmpty(patient.preferredPronouns) ? "" : patient.preferredPronouns;
                    command.Parameters.Add("@DOB", SqlDbType.Date).Value = patient.DOB;
                    command.Parameters.Add("@gender", SqlDbType.VarChar).Value = string.IsNullOrEmpty(patient.gender) ? "" : patient.gender;
                    command.Parameters.Add("@preferredLanguage", SqlDbType.VarChar).Value = patient.preferredLanguage;
                    command.Parameters.Add("@ethnicity", SqlDbType.VarChar).Value = patient.ethnicity;
                    command.Parameters.Add("@race", SqlDbType.VarChar).Value = patient.race;
                    command.Parameters.Add("@religion", SqlDbType.VarChar).Value = string.IsNullOrEmpty(patient.religion) ? "" : patient.religion;
                    command.Parameters.Add("@primaryPhysician", SqlDbType.VarChar).Value = patient.primaryPhysician;
                    command.Parameters.Add("@legalGuardian1", SqlDbType.VarChar).Value = string.IsNullOrEmpty(patient.legalGuardian1) ? "" : patient.legalGuardian1;
                    command.Parameters.Add("@legalGuardian2", SqlDbType.VarChar).Value = string.IsNullOrEmpty(patient.legalGuardian2) ? "" : patient.legalGuardian2;
                    command.Parameters.Add("@previousName", SqlDbType.VarChar).Value = string.IsNullOrEmpty(patient.previousName) ? "" : patient.previousName;
                    command.Parameters.Add("@genderAssignedAtBirth", SqlDbType.VarChar).Value = patient.genderAssignedAtBirth;
                    command.Parameters.Add("@patientImage", SqlDbType.VarChar).Value = string.IsNullOrEmpty(patient.patientImage) ? "" : patient.patientImage;

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    TempData["SuccessMessage"] = "You have successfully created a patient!";
                }
            }
            ModelState.Clear();
            return View();
        }

        public IActionResult PatientOverview(int mhn)
        {
            PortalViewModel portalViewModel = new PortalViewModel();

            //Creating a new patientDemographic instance
            portalViewModel.PatientDemographic = new PatientDemographic();

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
                        portalViewModel.PatientDemographic.MHN = Convert.ToInt32(dataReader["MHN"]);
                        portalViewModel.PatientDemographic.firstName = Convert.ToString(dataReader["firstName"]);
                        portalViewModel.PatientDemographic.middleName = Convert.ToString(dataReader["middleName"]);
                        portalViewModel.PatientDemographic.lastName = Convert.ToString(dataReader["lastName"]);
                        portalViewModel.PatientDemographic.suffix = Convert.ToString(dataReader["suffix"]);
                        portalViewModel.PatientDemographic.preferredPronouns = Convert.ToString(dataReader["preferredPronouns"]);
                        //This is grabbing the date of birth from the database and converting it to date only. Somehow even though it is 
                        //Saved to the database as only a date it does not read as just a date so this converts it to dateOnly.
                        DateTime dateTime = DateTime.Parse(dataReader["DOB"].ToString());
                        portalViewModel.PatientDemographic.DOB = new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
                        portalViewModel.PatientDemographic.gender = Convert.ToString(dataReader["gender"]);
                        portalViewModel.PatientDemographic.preferredLanguage = Convert.ToString(dataReader["preferredLanguage"]);
                        portalViewModel.PatientDemographic.ethnicity = Convert.ToString(dataReader["ethnicity"]);
                        portalViewModel.PatientDemographic.race = Convert.ToString(dataReader["race"]);
                        portalViewModel.PatientDemographic.religion = Convert.ToString(dataReader["religion"]);
                        portalViewModel.PatientDemographic.primaryPhysician = Convert.ToInt32(dataReader["primaryPhysician"]);
                        //Gets the provider for this patient using the primary physician number that links to the providers table
                        portalViewModel.PatientDemographic.providers = _listService.GetProvidersByProviderId(portalViewModel.PatientDemographic.primaryPhysician);
                        portalViewModel.PatientDemographic.legalGuardian1 = Convert.ToString(dataReader["legalGuardian1"]);
                        portalViewModel.PatientDemographic.legalGuardian2 = Convert.ToString(dataReader["legalGuardian2"]);
                        portalViewModel.PatientDemographic.previousName = Convert.ToString(dataReader["previousName"]);
                        //Gets the contact info for this patient using the MHN that links to the contact info table
                        portalViewModel.PatientDemographic.genderAssignedAtBirth = Convert.ToString(dataReader["genderAssignedAtBirth"]);
                        portalViewModel.PatientDemographic.ContactId = _listService.GetContactByMHN(portalViewModel.PatientDemographic.MHN);
                        portalViewModel.PatientDemographic.patientImage = Convert.ToString(dataReader["patientImage"]);
                    }
                }

                ViewBag.Patient = portalViewModel.PatientDemographic;
                ViewBag.MHN = mhn;

                connection.Close();
            }
            return View(portalViewModel);
        }

        public IActionResult PatientVisits(int mhn)
        {
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = GetPatientByMHN(mhn);
            

            // New list to hold all the patients in the database.
            List<Visits> patientVisits = _listService.GetPatientVisitsByMHN(mhn);

            viewModel.Visits = patientVisits;
            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = mhn;

            return View(viewModel);
        }

        public IActionResult PatientAllergies(int mhn)
        {
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = GetPatientByMHN(mhn);
         
            // List to hold the patient's list of allergies.
            List<PatientAllergies> allergies = new List<PatientAllergies>();

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                // Sql query.
                string sql = "SELECT * FROM [dbo].[PatientAllergies] WHERE MHN = @mhn ORDER BY PatientAllergyId ASC";

                SqlCommand cmd = new SqlCommand(sql, connection);

                // Replace placeholder with paramater to avoid sql injection.
                cmd.Parameters.AddWithValue("@mhn", mhn);


                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        // Create a new allergy object for each record.
                        PatientAllergies allergy = new PatientAllergies();

                        // Populate the allergy object with data from the database.

                        allergy.MHN = Convert.ToInt32(dataReader["MHN"]);
                        allergy.patientAllergyId = Convert.ToInt32(dataReader["PatientAllergyId"]);



                        allergy.allergyId = Convert.ToInt32(dataReader["AllergyId"]);

                        //Gets the provider for this patient using the primary physician number that links to the providers table
                        allergy.allergies = _listService.GetAllergyByAllergyId(allergy.allergyId);




                        // Fetch the DateTime value from the database and convert it to DateOnly
                        DateTime onSetDateTime = dataReader.GetDateTime(dataReader.GetOrdinal("onSetDate"));
                        DateOnly onSetDate = new DateOnly(onSetDateTime.Year, onSetDateTime.Month, onSetDateTime.Day);
                        allergy.onSetDate = onSetDate;

                        // Add the patient to the list
                        allergies.Add(allergy);
                    }
                }

                viewModel.PatientAllergies = allergies;
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
                        patientDemographic.patientImage = Convert.ToString(dataReader["patientImage"]);
                        patientDemographic.Active = Convert.ToBoolean(dataReader["Active"]);
                    }
                }

                connection.Close();
            }

            return patientDemographic;
        }





        public IActionResult PatientInsurance(int mhn)
        {
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = GetPatientByMHN(mhn);

            // List to hold the patient's list of allergies.
            List<PatientInsurance> insurances = new List<PatientInsurance>();

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                // Sql query.
                string sql = "SELECT * FROM [dbo].[PatientInsurance] WHERE MHN = @mhn ORDER BY active DESC, priority ASC";

                SqlCommand cmd = new SqlCommand(sql, connection);

                // Replace placeholder with paramater to avoid sql injection.
                cmd.Parameters.AddWithValue("@mhn", mhn);


                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    { 
                        // Create a new allergy object for each record.
                        PatientInsurance insurance = new PatientInsurance();

                        // Populate the allergy object with data from the database.
                        insurance.active = Convert.ToBoolean(dataReader["active"]);
                        //insurance name
                        insurance.providerName = Convert.ToString(dataReader["providerName"]);
                        insurance.memberId = Convert.ToString(dataReader["memberId"]);
                        insurance.policyNumber = Convert.ToString(dataReader["policyNumber"]);
                        insurance.groupNumber = Convert.ToString(dataReader["groupNumber"]);
                        insurance.priority = Convert.ToString(dataReader["priority"]);
                        insurance.primaryPhysician = Convert.ToInt32(dataReader["primaryPhysician"]);
                        insurance.providers = _listService.GetProvidersByProviderId(insurance.primaryPhysician);

                        // Add the insurance to the list
                        insurances.Add(insurance);
                    }
                }

                viewModel.PatientInsurance = insurances;
                ViewBag.Patient = viewModel.PatientDemographic;
                ViewBag.MHN = mhn;
                connection.Close();
            }

            return View(viewModel);
        }

        public IActionResult PatientCarePlan(int mhn)
        {
            //This will set the banner up and the view model so we can view everything
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = GetPatientByMHN(mhn);

            //This will grab a list of the care plans from the list services for the patient.
            List<CarePlan> carePlans = _listService.GetCarePlanByMHN(mhn);

            //This will add all of the data to a view bag that will be grabbed else where to display data correctly.
            viewModel.CarePlans = carePlans;
            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = mhn;

            return View(viewModel);
        }

        public IActionResult PatientVitals(int mhn)
        {
            // Needed to work with the patient banner properly.
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = GetPatientByMHN(mhn);

            // List to hold the patient's list of allergies.
            List<Vitals> vitals = new List<Vitals>();

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                // Sql query.
                string sql = "SELECT * FROM [dbo].[Vitals] WHERE patientId = @mhn";

                SqlCommand cmd = new SqlCommand(sql, connection);

                // Replace placeholder with paramater to avoid sql injection.
                cmd.Parameters.AddWithValue("@mhn", mhn);


                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        // Create a new allergy object for each record.
                        Vitals vital = new Vitals();

                        // Populate the vital object with data from the database.
                        vital.patientId = dataReader.GetInt32("patientID");
                        // Single visit where the vitals were taken.
                        vital.visits = _listService.GetVisitByVisitId(dataReader.GetInt32("visitId"));
                        vital.painLevel = dataReader.GetInt32("painLevel");
                        vital.temperature = dataReader.GetDecimal("temperature");
                        vital.bloodPressure = dataReader.GetInt32("bloodPressure");
                        vital.respiratoryRate = dataReader.GetInt32("respiratoryRate");
                        vital.pulseOximetry = dataReader.GetInt32("pulseOximetry");
                        vital.heightInches = dataReader.GetDecimal("heightInches");
                        vital.weightPounds = dataReader.GetDecimal("weightPounds");
                        vital.BMI = dataReader.GetDecimal("BMI");
                        vital.intakeMilliLiters = dataReader.GetInt32("intakeMilliliters");
                        vital.outputMilliLiters = dataReader.GetInt32("outputMilliliters");

                        // Add the vital to the list
                        vitals.Add(vital);
                    }
                }

                // Order the list by visit date descending
                viewModel.Vitals = vitals.OrderByDescending(v => v.visits.date).ToList();

                ViewBag.Patient = viewModel.PatientDemographic;
                ViewBag.MHN = mhn;

                connection.Close();
            }

            return View(viewModel);
        }



        [HttpPost]
        public IActionResult UpdateActiveStatus(int mhn, bool activeStatus)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                // Sql query.
                string sql = "UPDATE [dbo].[PatientDemographic] SET Active = @active WHERE MHN = @mhn";

                SqlCommand cmd = new SqlCommand(sql, connection);

                // Replace placeholder with parameter to avoid SQL injection.
                cmd.Parameters.AddWithValue("@mhn", mhn);
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


        [HttpPost]
        public IActionResult DeletePatient(int mhn)
        {

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                // Begin a transaction
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // SQL query to delete the patient and related records
                    string deletePatientSql = "DELETE FROM [dbo].[PatientDemographic] WHERE MHN = @mhn";

                    // All records related to the patient in some way or another that also must be deleted before we can.
                    string deleteRelatedRecordsSql = @"
                        DELETE FROM [dbo].[LabResults] WHERE MHN = @mhn;
                        DELETE FROM [dbo].[Vitals] WHERE visitId IN (SELECT visitId FROM [dbo].[Visits] WHERE MHN = @mhn);
    
                        DELETE FROM [dbo].[LabOrders] WHERE visitsId IN (SELECT visitsId FROM [dbo].[Visits] WHERE MHN = @mhn);
                        DELETE FROM [dbo].[MedOrders] WHERE visitId IN (SELECT visitId FROM [dbo].[Visits] WHERE MHN = @mhn);
                        DELETE FROM [dbo].[Visits] WHERE MHN = @mhn;
    
                        DELETE FROM [dbo].[MedAdministrationHistory] WHERE MHN = @mhn;
                        DELETE FROM [dbo].[PatientAllergies] WHERE MHN = @mhn;
                        DELETE FROM [dbo].[PatientInsurance] WHERE MHN = @mhn;
                        DELETE FROM [dbo].[Alerts] WHERE MHN = @mhn;
                        DELETE FROM [dbo].[PatientProblems] WHERE MHN = @mhn;
                        DELETE FROM [dbo].[PatientNotes] WHERE MHN = @mhn;
                        DELETE FROM [dbo].[PatientMedications] WHERE MHN = @mhn;
                        DELETE FROM [dbo].[PatientContact] WHERE MHN = @mhn;
                        DELETE FROM [dbo].[CarePlan] WHERE MHN = @mhn;
                    ";


                    // First, delete related records
                    using (SqlCommand cmd = new SqlCommand(deleteRelatedRecordsSql, connection, transaction))
                    {
                        // Set parameters if needed
                        cmd.Parameters.AddWithValue("@mhn", mhn);

                        int relatedRowsAffected = cmd.ExecuteNonQuery();
                    }

                    // Now, that everything related to the patient is deleted, delete the patient.
                    using (SqlCommand cmd = new SqlCommand(deletePatientSql, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@mhn", mhn);

                        int patientRowsAffected = cmd.ExecuteNonQuery();

                        // Check if the patient was deleted
                        if (patientRowsAffected <= 0)
                        {
                            throw new Exception("Patient with MHN " + mhn + " not found.");
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
                    return BadRequest("Failed to delete patient.");
                }
            }
        }


        public IActionResult PatientNotes(int mhn)
        {
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = GetPatientByMHN(mhn);

            // List to hold the patient's list of allergies.
            List<PatientNotes> notes = new List<PatientNotes>();

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                // Sql query.
                string sql = "SELECT * FROM [dbo].[PatientNotes] WHERE MHN = @mhn ORDER BY occurredOn DESC, createdAt DESC";

                SqlCommand cmd = new SqlCommand(sql, connection);

                // Replace placeholder with paramater to avoid sql injection.
                cmd.Parameters.AddWithValue("@mhn", mhn);


                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        // Create a new allergy object for each record.
                        PatientNotes note = new PatientNotes();

                        // Populate the note object with data from the database.
                        //TODO: make this get the right date only data type.
                        //note.occurredOn = Convert.ToString(dataReader["occurrdeOn"]);
                        note.occurredOn = DateOnly.FromDateTime(dataReader.GetDateTime(dataReader.GetOrdinal("occurredOn")));
                        note.createdAt = Convert.ToDateTime(dataReader["createdAt"]);
                        note.providers = _listService.GetProvidersByProviderId(Convert.ToInt32(dataReader["createdBy"]));
                        note.assocProvider = _listService.GetProvidersByProviderId(Convert.ToInt32(dataReader["associatedProvider"]));
                        note.category = Convert.ToString(dataReader["category"]);
                        note.Note = Convert.ToString(dataReader["note"]);


                        // Add the insurance to the list
                        notes.Add(note);
                    }
                }

                viewModel.PatientNotes = notes;
                ViewBag.Patient = viewModel.PatientDemographic;
                ViewBag.MHN = mhn;
                connection.Close();
            }

            return View(viewModel);
        }
    }
}
