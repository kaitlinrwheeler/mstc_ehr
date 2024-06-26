﻿using EHRApplication.Models;
using EHRApplication.Services;
using EHRApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
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
                string sql = "SELECT MHN, firstName, lastName, DOB, gender, Active FROM [dbo].[PatientDemographic] ORDER BY Active DESC, MHN DESC";

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

        [Authorize(Roles = "Admin")]
        public IActionResult Index(int mhn)
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(PatientDemographic patient)
        {
            // Testing to see if the date of birth entered was a future date or not
            if (patient.DOB >= DateOnly.FromDateTime(DateTime.Now))
            {
                // Adding an error to the DOB model to display an error.
                ModelState.AddModelError("DOB", "Date cannot be in the future.");
                return View(patient);
            }

            // Returns the model if null because there were errors in validating it
            if (!ModelState.IsValid)
            {
                return View(patient);
            }

            // check if the file is too large
            if (patient.patientImageFile != null && patient.patientImageFile.Length > 4 * 1024 * 1024)
            {
                ModelState.AddModelError("patientImageFile", "File size must be less than 4MB.");
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
                    await patient.patientImageFile.CopyToAsync(stream);
                }

                // save filepath to pt image property
                patient.patientImage = fileName;

            }

            // process race selection
            if (patient.race == null && patient.raceList.Contains("Other") && !string.IsNullOrEmpty(patient.OtherRace))
            {
                //removes all elements form race list that equal the parameter given in this case that is Other
                patient.raceList.RemoveAll(r => string.Equals(r, "Other", StringComparison.OrdinalIgnoreCase));
                patient.raceList.Add(patient.OtherRace);
            }

            patient.race = string.Join(", ", patient.raceList);

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

                //This will go and get the most recent entry into the database which will be the one that we just entered.
                sql = "SELECT MHN FROM [PatientDemographic] WHERE firstName = @firstName AND lastName = @lastName AND DOB = @DOB AND gender = @gender";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    //Setting parameters for parameterized queries.
                    command.Parameters.Add("@firstName", SqlDbType.VarChar).Value = patient.firstName;
                    command.Parameters.Add("@lastName", SqlDbType.VarChar).Value = patient.lastName;
                    command.Parameters.Add("@DOB", SqlDbType.Date).Value = patient.DOB;
                    command.Parameters.Add("@gender", SqlDbType.VarChar).Value = patient.gender;
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            patient.MHN = Convert.ToInt32(reader["MHN"]);
                        }
                    }
                    connection.Close();
                }
            }
            ModelState.Clear();
            return RedirectToAction("PatientOverview", new {mhn = patient.MHN});
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
                        portalViewModel.PatientDemographic.HasAlerts = _listService.CheckPatientAlerts(portalViewModel.PatientDemographic.MHN);
                    }
                }

                ViewBag.Patient = portalViewModel.PatientDemographic;
                ViewBag.MHN = mhn;

                connection.Close();
            }
            return View(portalViewModel);
        }

        public IActionResult PatientAllergies(int mhn)
        {
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = _listService.GetPatientByMHN(mhn);

            // List to hold the patient's list of allergies.
            List<PatientAllergies> allergies = new List<PatientAllergies>();

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                // Sql query.
                string sql = "SELECT * FROM [dbo].[PatientAllergies] WHERE MHN = @mhn ORDER BY activeStatus DESC, onSetDate DESC";

                SqlCommand cmd = new SqlCommand(sql, connection);

                // Replace placeholder with paramater to avoid sql injection.
                cmd.Parameters.AddWithValue("@mhn", mhn);


                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        // create a new allergy object for each record
                        PatientAllergies allergy = new PatientAllergies();

                        // populate the allergy object with data from the database
                        allergy.MHN = Convert.ToInt32(dataReader["MHN"]);
                        allergy.patientAllergyId = Convert.ToInt32(dataReader["PatientAllergyId"]);
                        allergy.allergyId = Convert.ToInt32(dataReader["AllergyId"]);

                        // uses allergy ID to get allergy names from the allergy table
                        allergy.allergies = _listService.GetAllergyByAllergyId(allergy.allergyId);

                        // Fetch the DateTime value from the database and convert it to DateOnly
                        DateTime onSetDateTime = dataReader.GetDateTime(dataReader.GetOrdinal("onSetDate"));
                        DateOnly onSetDate = new DateOnly(onSetDateTime.Year, onSetDateTime.Month, onSetDateTime.Day);
                        allergy.onSetDate = onSetDate;

                        allergy.activeStatus = Convert.ToBoolean(dataReader["activeStatus"]);

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


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult CreatePatientInsurance(int mhn)
        {
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = _listService.GetPatientByMHN(mhn);

            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = mhn;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreatePatientInsurance(PatientInsurance insurance)
        {
            if (insurance.primaryPhysician == -1)
            {
                ModelState.AddModelError("insurance.primaryPhysician", "Please select a physician.");
            }

            // Need to make sure these don't get validated since they don't have anything to do with the form. 
            ModelState.Remove("insurance.patients");
            ModelState.Remove("insurance.providers");

            if (!ModelState.IsValid)
            {
                PortalViewModel portalViewModel = new PortalViewModel();
                portalViewModel.PatientDemographic = _listService.GetPatientByMHN(insurance.MHN);
                portalViewModel.PatientInsurance = insurance;
                ViewBag.Patient = portalViewModel.PatientDemographic;
                ViewBag.MHN = insurance.MHN;

                return View(portalViewModel);
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Insert a new record into the LabTestProfile table
                string sql = @"INSERT INTO [dbo].[PatientInsurance]
                           (MHN, providerName, memberId, policyNumber, groupNumber, priority, primaryPhysician, active)
                           VALUES 
                           (@MHN, @providerName, @memberId, @policyNumber, @groupNumber, @priority, @primaryPhysician, @active)";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@MHN", insurance.MHN);
                cmd.Parameters.AddWithValue("@providerName", insurance.providerName);
                cmd.Parameters.AddWithValue("@memberId", insurance.memberId);
                cmd.Parameters.AddWithValue("@policyNumber", insurance.policyNumber);
                cmd.Parameters.AddWithValue("@groupNumber", string.IsNullOrEmpty(insurance.groupNumber) ? DBNull.Value : insurance.groupNumber);
                cmd.Parameters.AddWithValue("@priority", insurance.priority);
                cmd.Parameters.AddWithValue("@primaryPhysician", insurance.primaryPhysician); 
                cmd.Parameters.AddWithValue("@active", insurance.active);


                cmd.ExecuteNonQuery();

                connection.Close();
            }

            return RedirectToAction("PatientInsurance", new {mhn = insurance.MHN});
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult EditPatientInsurance(int insuranceId)
        {
            PatientInsurance insurance = new PatientInsurance();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Select the record from the PatientInsurance table based on insuranceId
                string sql = @"SELECT MHN, providerName, memberId, policyNumber, groupNumber, priority, primaryPhysician, active
                       FROM [dbo].[PatientInsurance]
                       WHERE patientInsuranceId = @insuranceId";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@insuranceId", insuranceId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        insurance.patientInsuranceId = insuranceId;
                        insurance.MHN = Convert.ToInt32(reader["MHN"]);
                        insurance.providerName = reader["providerName"].ToString();
                        insurance.memberId = reader["memberId"].ToString();
                        insurance.policyNumber = reader["policyNumber"].ToString();
                        insurance.groupNumber = reader["groupNumber"].ToString();
                        insurance.priority = reader["priority"].ToString();
                        insurance.primaryPhysician = Convert.ToInt32(reader["primaryPhysician"]);
                        insurance.active = Convert.ToBoolean(reader["active"]);
                    }
                }

                connection.Close();
            }
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientInsurance = insurance;
            viewModel.PatientDemographic = _listService.GetPatientByMHN(insurance.MHN);

            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = insurance.MHN;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult EditPatientInsurance(PatientInsurance insurance)
        {
            if (insurance.primaryPhysician == -1)
            {
                ModelState.AddModelError("insurance.primaryPhysician", "Please select a physician.");
            }

            // Need to make sure these don't get validated since they don't have anything to do with the form. 
            ModelState.Remove("insurance.patients");
            ModelState.Remove("insurance.providers");

            if (insurance.primaryPhysician == -1)
            {
                ModelState.AddModelError("insurance.primaryPhysician", "Please select a physician.");
            }

            if (!ModelState.IsValid)
            {
                PortalViewModel portalViewModel = new PortalViewModel();
                portalViewModel.PatientDemographic = _listService.GetPatientByMHN(insurance.MHN);
                portalViewModel.PatientInsurance = insurance;
                ViewBag.Patient = portalViewModel.PatientDemographic;
                ViewBag.MHN = insurance.MHN;

                return View(portalViewModel);
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Update the existing record in the PatientInsurance table
                string sql = @"UPDATE [dbo].[PatientInsurance] 
                       SET providerName = @providerName, 
                           memberId = @memberId, 
                           policyNumber = @policyNumber, 
                           groupNumber = @groupNumber, 
                           priority = @priority, 
                           primaryPhysician = @primaryPhysician, 
                           active = @active
                       WHERE patientInsuranceId = @insuranceId";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@insuranceId", insurance.patientInsuranceId);
                cmd.Parameters.AddWithValue("@providerName", insurance.providerName);
                cmd.Parameters.AddWithValue("@memberId", insurance.memberId);
                cmd.Parameters.AddWithValue("@policyNumber", insurance.policyNumber);
                cmd.Parameters.AddWithValue("@groupNumber", string.IsNullOrEmpty(insurance.groupNumber) ? DBNull.Value : insurance.groupNumber);
                cmd.Parameters.AddWithValue("@priority", insurance.priority);
                cmd.Parameters.AddWithValue("@primaryPhysician", insurance.primaryPhysician);
                cmd.Parameters.AddWithValue("@active", insurance.active);

                cmd.ExecuteNonQuery();

                connection.Close();
            }

            return RedirectToAction("PatientInsurance", new { mhn = insurance.MHN });
        }

        public IActionResult PatientInsurance(int mhn)
        {
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = _listService.GetPatientByMHN(mhn);

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
                        insurance.patientInsuranceId = Convert.ToInt32(dataReader["patientInsuranceId"]);
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

                viewModel.PatientInsurances = insurances;
                ViewBag.Patient = viewModel.PatientDemographic;
                ViewBag.MHN = mhn;
                connection.Close();
            }

            return View(viewModel);
        }

        public IActionResult PatientCarePlan(int mhn)
        {
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = _listService.GetPatientByMHN(mhn);

            List<CarePlan> carePlans = _listService.GetCarePlanByMHN(mhn);

            // Check if end date has passed for each care plan
            foreach (var carePlan in carePlans)
            {
                if (carePlan.endDate < DateTime.Today)
                {
                    carePlan.active = false;
                    // Update the database with modified care plan data
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(this._connectionString))
                        {
                            // SQL query that is going to update the data in the database table.
                            string sql = "UPDATE [CarePlan] active = @active " +
                                         "WHERE CPId = @CPId";

                            using (SqlCommand command = new SqlCommand(sql, connection))
                            {
                                command.CommandType = CommandType.Text;

                                // Adding parameters
                                command.Parameters.Add("@active", SqlDbType.Bit).Value = carePlan.active;
                                command.Parameters.Add("@CPId", SqlDbType.Int).Value = carePlan.CPId;

                                connection.Open();
                                command.ExecuteNonQuery();
                                connection.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle the exception as required
                        Debug.WriteLine(ex.Message);
                    }
                }
            }

            viewModel.CarePlans = carePlans;
            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = mhn;

            return View(viewModel);
        }

        public IActionResult PatientVitals(int mhn)
        {
            // Needed to work with the patient banner properly.
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = _listService.GetPatientByMHN(mhn);

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
                        vital.vitalsId = dataReader.GetInt32("vitalsId");
                        // Single visit where the vitals were taken.
                        vital.visits = _listService.GetVisitByVisitId(dataReader.GetInt32("visitId"));
                        vital.painLevel = dataReader.GetInt32("painLevel");
                        vital.temperature = dataReader.GetDecimal("temperature");
                        vital.bloodPressure = dataReader.GetString("bloodPressure");
                        vital.respiratoryRate = dataReader.GetInt32("respiratoryRate");
                        vital.pulseOximetry = dataReader.GetDecimal("pulseOximetry");
                        vital.heightInches = dataReader.GetDecimal("heightInches");
                        vital.weightPounds = dataReader.GetDecimal("weightPounds");
                        vital.BMI = dataReader.GetDecimal("BMI");
                        vital.intakeMilliLiters = dataReader.GetInt32("intakeMilliliters");
                        vital.outputMilliLiters = dataReader.GetInt32("outputMilliliters");
                        vital.pulse = dataReader.GetInt32("pulse");

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
        [Authorize(Roles = "Admin")]
        public IActionResult UpdatePatientActiveStatus(int mhn, bool activeStatus)
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
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateInsuranceActiveStatus(int insuranceId, bool activeStatus)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                // Sql query.
                string sql = "UPDATE [dbo].[PatientInsurance] SET active = @active WHERE patientInsuranceId = @insuranceId";

                SqlCommand cmd = new SqlCommand(sql, connection);

                // Replace placeholder with parameter to avoid SQL injection.
                cmd.Parameters.AddWithValue("@insuranceId", insuranceId);
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
        [Authorize(Roles = "Admin")]
        public IActionResult DeletePatient(int mhn)
        {

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                // Begin a transaction
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // get patient image file name
                    string getImageSql = "SELECT patientImage FROM [dbo].[PatientDemographic] WHERE MHN = @mhn";
                    string patientImageFileName = string.Empty;

                    using (SqlCommand getImageCmd = new SqlCommand(getImageSql, connection, transaction))
                    {
                        getImageCmd.Parameters.AddWithValue("@mhn", mhn);
                        using (var reader = getImageCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // check if db value is null before assigning
                                patientImageFileName = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                            }
                        }
                    }

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

                    // delete the patient image file if it exists
                    var imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    var imageFilePath = Path.Combine(imageDirectory, patientImageFileName);

                    // check if file exists before deleting
                    if (System.IO.File.Exists(imageFilePath))
                    {
                        System.IO.File.Delete(imageFilePath);
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
            viewModel.PatientDemographic = _listService.GetPatientByMHN(mhn);

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

                        // Populate the patientNote object with data from the database.
                        //TODO: make this get the right date only data type.
                        //patientNote.occurredOn = Convert.ToString(dataReader["occurredOn"]);
                        note.occurredOn = DateOnly.FromDateTime(dataReader.GetDateTime(dataReader.GetOrdinal("occurredOn")));
                        note.createdAt = Convert.ToDateTime(dataReader["createdAt"]);
                        note.providers = _listService.GetProvidersByProviderId(Convert.ToInt32(dataReader["createdBy"]));
                        note.assocProvider = _listService.GetProvidersByProviderId(Convert.ToInt32(dataReader["associatedProvider"]));
                        note.category = Convert.ToString(dataReader["category"]);
                        note.Note = Convert.ToString(dataReader["note"]);
                        note.patientNotesId = Convert.ToInt32(dataReader["patientNotesId"]);


                        // Add the insurance to the list
                        notes.Add(note);
                    }
                }
            }
            viewModel.PatientNotes = notes;
            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = mhn;

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult EditPatientForm(int mhn)
        {
            PortalViewModel viewModel = new PortalViewModel();

            viewModel.PatientDemographic = _listService.GetPatientByMHN(mhn);

            // Define the list of races to remove
            List<string> racesToRemove = new List<string>
            {
                "Asian",
                "American Indian or Alaska Native",
                "Black or African American",
                "White"
            };
            // Remove the specified races from the race field
            var otherRace = string.Join(", ", viewModel.PatientDemographic.race.Split(',')
                .Select(r => r.Trim())
                .Where(r => !racesToRemove.Contains(r))
                .ToList());

            //This will see if the data in the table was entered using the other table text input and display that for the user.
            if(viewModel.PatientDemographic.gender != "Male" && viewModel.PatientDemographic.gender != "Female" && viewModel.PatientDemographic.gender != "Non-binary")
            {
                //Setting the other gender object equal to what is in the table as there is no other gender column in the table
                viewModel.PatientDemographic.OtherGender = viewModel.PatientDemographic.gender;
                viewModel.PatientDemographic.gender = "Other";
            }
            if (viewModel.PatientDemographic.preferredPronouns != "He/Him" && viewModel.PatientDemographic.preferredPronouns != "She/Her" && viewModel.PatientDemographic.preferredPronouns != "They/Them")
            {
                //Setting the other pronouns object equal to what is in the table as there is no other pronouns column in the table
                viewModel.PatientDemographic.OtherPronouns = viewModel.PatientDemographic.preferredPronouns;
                viewModel.PatientDemographic.preferredPronouns = "Other";
            }
            if (!otherRace.IsNullOrEmpty())
            {
                //Setting the other race if there was an input entered using the other race input
                viewModel.PatientDemographic.OtherRace = otherRace;
                //removes data from the race.
                viewModel.PatientDemographic.race = string.Join(", ", viewModel.PatientDemographic.race.Split(',').Select(r => r.Trim()).Where(r => !otherRace.Contains(r)).ToList());
                viewModel.PatientDemographic.race += ",Other";
            }
            if(viewModel.PatientDemographic.preferredPronouns == "Other")
            {
                viewModel.PatientDemographic.preferredPronouns = viewModel.PatientDemographic.OtherPronouns;
            }
            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = mhn;
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult EditPatientForm(PatientDemographic patient)
        {
            PortalViewModel viewModel = new PortalViewModel();

            viewModel.PatientDemographic = patient;
            ViewBag.Patient = _listService.GetPatientByMHN(patient.MHN);
            ViewBag.MHN = patient.MHN;

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
                if (!patient.raceList.IsNullOrEmpty())
                {
                    patient.race = string.Join(", ", patient.raceList);
                }
                return View(viewModel);
            }

            // process race selection
            if (patient.race == null && patient.raceList.Contains("Other") && !string.IsNullOrEmpty(patient.OtherRace))
            {
                //removes all elements form race list that equal the parameter given in this case that is Other
                patient.raceList.RemoveAll(r => string.Equals(r, "Other", StringComparison.OrdinalIgnoreCase));
                patient.raceList.Add(patient.OtherRace);
            }
            
            patient.race = string.Join(", ", patient.raceList);

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
                string sql = @"UPDATE [PatientDemographic] 
                   SET firstName = @firstName, 
                       middleName = @middleName, 
                       lastName = @lastName, 
                       suffix = @suffix, 
                       preferredPronouns = @preferredPronouns, 
                       DOB = @DOB, 
                       gender = @gender, 
                       preferredLanguage = @preferredLanguage, 
                       ethnicity = @ethnicity, 
                       race = @race, 
                       religion = @religion, 
                       primaryPhysician = @primaryPhysician, 
                       legalGuardian1 = @legalGuardian1, 
                       legalGuardian2 = @legalGuardian2, 
                       previousName = @previousName, 
                       genderAssignedAtBirth = @genderAssignedAtBirth
                   WHERE MHN = @MHN";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;

                    //The some of them test to see if the value if null or empty because they are optional on the form so if it is null or empty it will display NA otherwise will add the data enterd by the user.
                    //adding parameters
                    command.Parameters.Add("@MHN", SqlDbType.Int).Value = patient.MHN;
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

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            ModelState.Clear();
            return RedirectToAction("PatientOverview", "Patient", new {mhn = patient.MHN});
        }

        // takes search term and returns relevant results
        public IActionResult Search(string searchTerm)
        {
            List<PatientDemographic> searchResults = new List<PatientDemographic>();

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;

                // checks if searchTerm is int, sets query based on term type
                if (int.TryParse(searchTerm, out int mhn))
                {
                    // selects pt by MHN if it's a match
                    cmd.CommandText = "SELECT * FROM [dbo].[PatientDemographic] WHERE MHN = @mhn";
                    cmd.Parameters.AddWithValue("@mhn", mhn);
                }
                else
                {
                    // accepts string fragments, returns similar results
                    cmd.CommandText = @"SELECT * FROM [dbo].[PatientDemographic] 
                                WHERE firstName LIKE @searchTerm 
                                OR lastName LIKE @searchTerm 
                                OR middleName LIKE @searchTerm 
                                OR suffix LIKE @searchTerm";
                    cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                }

                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        DateTime dateTime = DateTime.Parse(dataReader["DOB"].ToString());
                        PatientDemographic patient = new PatientDemographic
                        {
                            MHN = Convert.ToInt32(dataReader["MHN"]),
                            firstName = Convert.ToString(dataReader["firstName"]),
                            middleName = Convert.ToString(dataReader["middleName"]),
                            lastName = Convert.ToString(dataReader["lastName"]),
                            suffix = Convert.ToString(dataReader["suffix"]),
                            DOB = new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day),
                            gender = Convert.ToString(dataReader["gender"]),
                            Active = Convert.ToBoolean(dataReader["Active"]),
                        };

                        // adds results to list
                        searchResults.Add(patient);
                    }
                }
            }

            // returns any results found
            return View("PatientSearchResults", searchResults);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult PatientAlerts(int mhn)
        {
            // Needed to work with the patient banner properly.
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = _listService.GetPatientByMHN(mhn);

            // List to hold the patient's list of allergies.
            List<Alerts> alerts = new List<Alerts>();

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                // Sql query.
                string sql = "SELECT * FROM [dbo].[Alerts] WHERE MHN = @mhn ORDER BY activeStatus DESC, startDate DESC, endDate DESC";

                SqlCommand cmd = new SqlCommand(sql, connection);

                // Replace placeholder with paramater to avoid sql injection.
                cmd.Parameters.AddWithValue("@mhn", mhn);


                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        // Create a new allergy object for each record.
                        Alerts alert = new Alerts();

                        // Populate the alert object with data from the database.
                        alert.alertId = dataReader.GetInt32("alertId");
                        alert.alertName = dataReader.GetString("alertName");
                        alert.startDate = dataReader.GetDateTime("startDate");

                        alert.endDate = dataReader.GetDateTime("endDate");
                        alert.activeStatus = dataReader.GetBoolean("activeStatus");

                        // If the alert should be inactive.
                        if(alert.activeStatus == true && alert.endDate < DateTime.Now)
                        {
                            // Call function that updates the alert status to be inactive.
                            if (SetAlertInactive(alert.alertId))
                            {
                                // Set active status to inactive here since we just changed it in the database.
                                alert.activeStatus = false;
                            }
                            else
                            {
                                // Set an error message
                                // TODO: should log it here.
                            }
                        }

                        // Add the alert to the list
                        alerts.Add(alert);
                    }
                }

                // Order the list by start date newest to oldest.
                viewModel.Alerts = alerts
                    .OrderByDescending(a => a.activeStatus)
                    .ThenByDescending(a => a.startDate)
                    .ThenByDescending(a => a.endDate)
                    .ToList();

                ViewBag.Patient = viewModel.PatientDemographic;
                ViewBag.MHN = mhn;

                connection.Close();
            }

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        public bool SetAlertInactive(int id)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                // Sql query.
                string sql = "UPDATE [dbo].[Alerts] SET activeStatus = 'false' WHERE alertId = @id";

                SqlCommand cmd = new SqlCommand(sql, connection);

                // Replace placeholder with parameter to avoid SQL injection.
                cmd.Parameters.AddWithValue("@id", id);

                // Execute the SQL command.
                int rowsAffected = cmd.ExecuteNonQuery();

                connection.Close();

                // Check if any rows were affected.
                if (rowsAffected > 0)
                {
                    // Successfully updated.
                    return true;
                }
                else
                {
                    // No rows were affected, return an error message.
                    return false;
                }
            }

        }

        [Authorize(Roles = "Admin")]
        public IActionResult CreateVitalsForm(int mhn)
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
        public IActionResult CreateVitalsForm(Vitals vital)
        {
            if (vital.visitId == 0)
            {
                ModelState.AddModelError("Vital.visitId", "Please select a visit.");
            }
            //returns the model if null because there were errors in validating it
            if (!ModelState.IsValid)
            {
                PortalViewModel viewModel = new PortalViewModel();
                viewModel.PatientDemographic = _listService.GetPatientByMHN(vital.patientId);
                viewModel.Vital = vital;
                ViewBag.Patient = viewModel.PatientDemographic;
                ViewBag.MHN = vital.patientId;

                return View(viewModel);
            }
            else if (vital.patientId != 0)
            {
                //Calculate the BMI
                vital.BMI = _listService.BMICalculator(vital.weightPounds, vital.heightInches);
                //go to the void list service that will input the data into the database.
                _listService.InsertIntoVitals(vital);
            }

            return RedirectToAction("PatientVitals", new { mhn = vital.patientId });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult EditVitalsForm(int vitalsId)
        {
            // Needed to work with the patient banner properly.
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.Vital = _listService.GetVitalsByVitalsId(vitalsId);
            viewModel.PatientDemographic = _listService.GetPatientByMHN(viewModel.Vital.patientId);

            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = viewModel.Vital.patientId;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult EditVitalsForm(Vitals vital)
        {
            if (vital.visitId == 0)
            {
                ModelState.AddModelError("visitId", "Please select a vital record.");
            }
            //returns the model if null because there were errors in validating it
            if (!ModelState.IsValid)
            {
                PortalViewModel viewModel = new PortalViewModel();
                viewModel.PatientDemographic = _listService.GetPatientByMHN(vital.patientId);
                viewModel.Vital = vital;
                ViewBag.Patient = viewModel.PatientDemographic;
                ViewBag.MHN = vital.patientId;

                return View(viewModel);
            }
            else if (vital.patientId != 0)
            {
                //Calculate the BMI
                vital.BMI = _listService.BMICalculator(vital.weightPounds, vital.heightInches);
                //go to the void list service that will update the data into the database.
                _listService.UpdateVitals(vital);
            }

            return RedirectToAction("PatientVitals", new { mhn = vital.patientId });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult CreatePatientCarePlanForm(int mhn)
        {
            // Needed to work with the patient banner properly.
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = _listService.GetPatientByMHN(mhn);

            // Needed to work with the patient banner properly.
            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = mhn;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreatePatientCarePlanForm(CarePlan carePlan)
        {
            // Needed to work with the patient banner properly.
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = _listService.GetPatientByMHN(carePlan.MHN);
            viewModel.CarePlansDetails = carePlan;
            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = carePlan.MHN;


            if (carePlan.visitsId == -1)
            {
                ModelState.AddModelError("CarePlansDetails.visitsId", "Please select a visit.");
            }

            bool isPriorityValid = false;

            if (carePlan.priority == "Low" || carePlan.priority == "Medium" || carePlan.priority == "High")
            {
                isPriorityValid = true;
            }

            if (!isPriorityValid) { ModelState.AddModelError("CarePlansDetails.priority", "Please select a priority level."); }

            // Check to see if the date is more than 5 years in the past.
            if (carePlan.startDate <= DateTime.Now.AddYears(-5))
            {
                ModelState.AddModelError("CarePlansDetails.startDate", "Date cannot be in the future.");
                return View(viewModel);
            }

            // Check to make sure end date is after the start date.
            if (carePlan.endDate <= carePlan.startDate)
            {
                ModelState.AddModelError("CarePlansDetails.endDate", "End date must be after the start date.");
                return View(viewModel);
            }
            // Check to make sure the end date is not more than 2 years from today's date.
            else if (carePlan.endDate > DateTime.Now.AddYears(2))
            {
                ModelState.AddModelError("CarePlansDetails.endDate", "End date cannot be more than 2 years in the future.");
                return View(viewModel);
            }

            if (carePlan.title == null)
            {
                ModelState.AddModelError("CarePlansDetails.Title", "Title must not be empty.");
            }
            else if (!Regex.IsMatch(carePlan.title, @"^[a-zA-Z0-9\s/\-]+$"))
            {
                ModelState.AddModelError("CarePlansDetails.Title", "Title must only contain letters, numbers, and punctuation.");
            }

            if (carePlan.diagnosis == null)
            {
                ModelState.AddModelError("CarePlansDetails.Diagnosis", "Diagnosis must not be empty.");
            }
            else if (!Regex.IsMatch(carePlan.diagnosis, @"^[a-zA-Z0-9\s/\-]+$"))
            {
                ModelState.AddModelError("CarePlansDetails.Diagnosis", "Diagnosis must only contain letters, numbers, and punctuation.");
            }

            // Don't need to check for these since they aren't on the form.
            ModelState.Remove("visits");
            ModelState.Remove("visitsId");
            ModelState.Remove("patients");

            // Returns the model if there are validation errors.
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    //SQL query that is going to insert the data that the user entered into the database table.
                    string sql = "INSERT INTO [CarePlan] (MHN, priority, startDate, endDate, title, diagnosis, visitsId, active) " +
                        "VALUES (@mhn, @priority, @startDate, @endDate, @title, @diagnosis, @visitsId, @active)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;

                        //adding parameters
                        command.Parameters.Add("@mhn", SqlDbType.Int).Value = carePlan.MHN;
                        command.Parameters.Add("@priority", SqlDbType.NVarChar).Value = carePlan.priority;
                        command.Parameters.Add("@startDate", SqlDbType.DateTime2).Value = carePlan.startDate;
                        command.Parameters.Add("@endDate", SqlDbType.DateTime2).Value = carePlan.endDate;
                        command.Parameters.Add("@title", SqlDbType.NVarChar).Value = carePlan.title;
                        command.Parameters.Add("@diagnosis", SqlDbType.NVarChar).Value = carePlan.diagnosis;
                        command.Parameters.Add("@visitsId", SqlDbType.Int).Value = carePlan.visitsId;
                        command.Parameters.Add("@active", SqlDbType.Bit).Value = carePlan.active;

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            // Should also send an error message to user later or take to "oh no" page.
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            return RedirectToAction("PatientCarePlan", new { mhn = carePlan.MHN });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult EditPatientCarePlanForm(int carePlanId)
        {
            // Needed to work with the patient banner properly.
            PortalViewModel viewModel = new PortalViewModel();

            //Creating a new patientDemographic instance
            CarePlan carePlan = new CarePlan();

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                // Sql query to get the patient with the passed in mhn.
                string sql = "SELECT MHN, priority, startDate, endDate, active, title, diagnosis, visitsId " +
                    "FROM [dbo].[CarePlan] WHERE CPId = @CPId";

                SqlCommand cmd = new SqlCommand(sql, connection);

                // Replace placeholder with paramater to avoid sql injection.
                cmd.Parameters.AddWithValue("@CPId", carePlanId);

                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        carePlan.CPId = carePlanId;
                        carePlan.MHN = Convert.ToInt32(dataReader["MHN"]);
                        carePlan.patients = _listService.GetPatientByMHN(carePlan.MHN);

                        carePlan.priority = Convert.ToString(dataReader["priority"]);
                        carePlan.startDate = DateTime.Parse(dataReader["startDate"].ToString());
                        carePlan.endDate = DateTime.Parse(dataReader["endDate"].ToString());
                        carePlan.title = Convert.ToString(dataReader["title"]);
                        carePlan.diagnosis = Convert.ToString(dataReader["diagnosis"]);
                        carePlan.active = Convert.ToBoolean(dataReader["active"]);

                        carePlan.visitsId = Convert.ToInt32(dataReader["visitsId"]);
                    }
                }

                connection.Close();
            }

            // Needed to work with the patient banner properly.
            viewModel.PatientDemographic = _listService.GetPatientByMHN(carePlan.MHN);
            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = carePlan.MHN;

            viewModel.CarePlansDetails = carePlan;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult EditPatientCarePlanForm(CarePlan carePlan)
        {
            // Needed to work with the patient banner properly.
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = _listService.GetPatientByMHN(carePlan.MHN);
            viewModel.CarePlansDetails = carePlan;
            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = carePlan.MHN;


            if (carePlan.visitsId == -1)
            {
                ModelState.AddModelError("CarePlansDetails.visitsId", "Please select a visit.");
            }

            bool isPriorityValid = false;

            if (carePlan.priority == "Low" || carePlan.priority == "Medium" || carePlan.priority == "High")
            {
                isPriorityValid = true;
            }

            if (!isPriorityValid) { ModelState.AddModelError("CarePlansDetails.priority", "Please select a priority level."); }

            // Check to see if the date is more than 5 years in the past.
            if (carePlan.startDate <= DateTime.Now.AddYears(-5))
            {
                ModelState.AddModelError("CarePlansDetails.startDate", "Date cannot be in the future.");
                return View(viewModel);
            }

            // Check to make sure end date is after the start date.
            if (carePlan.endDate <= carePlan.startDate)
            {
                ModelState.AddModelError("CarePlansDetails.endDate", "End date must be after the start date.");
                return View(viewModel);
            }
            // Check to make sure the end date is not more than 2 years from today's date.
            else if (carePlan.endDate > DateTime.Now.AddYears(2))
            {
                ModelState.AddModelError("CarePlansDetails.endDate", "End date cannot be more than 2 years in the future.");
                return View(viewModel);
            }

            if (carePlan.title == null)
            {
                ModelState.AddModelError("CarePlansDetails.Title", "Title must not be empty.");
            }
            else if (!Regex.IsMatch(carePlan.title, @"^[a-zA-Z0-9\s/\-]+$"))
            {
                ModelState.AddModelError("CarePlansDetails.Title", "Title must only contain letters, numbers, and punctuation.");
            }

            if (carePlan.diagnosis == null)
            {
                ModelState.AddModelError("CarePlansDetails.Diagnosis", "Diagnosis must not be empty.");
            }
            else if (!Regex.IsMatch(carePlan.diagnosis, @"^[a-zA-Z0-9\s/\-]+$"))
            {
                ModelState.AddModelError("CarePlansDetails.Diagnosis", "Diagnosis must only contain letters, numbers, and punctuation.");
            }

            // Don't need to check for these since they aren't on the form.
            ModelState.Remove("visits");
            ModelState.Remove("visitsId");
            ModelState.Remove("patients");

            // Returns the model if there are validation errors.
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    // SQL query that is going to update the data in the database table.
                    string sql = "UPDATE [CarePlan] SET priority = @priority, startDate = @startDate, endDate = @endDate, " +
                                 "title = @title, diagnosis = @diagnosis, visitsId = @visitsId, active = @active " +
                                 "WHERE CPId = @CPId";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;

                        // Adding parameters
                        command.Parameters.Add("@priority", SqlDbType.NVarChar).Value = carePlan.priority;
                        command.Parameters.Add("@startDate", SqlDbType.DateTime2).Value = carePlan.startDate;
                        command.Parameters.Add("@endDate", SqlDbType.DateTime2).Value = carePlan.endDate;
                        command.Parameters.Add("@title", SqlDbType.NVarChar).Value = carePlan.title;
                        command.Parameters.Add("@diagnosis", SqlDbType.NVarChar).Value = carePlan.diagnosis;
                        command.Parameters.Add("@visitsId", SqlDbType.Int).Value = carePlan.visitsId;
                        command.Parameters.Add("@active", SqlDbType.Bit).Value = carePlan.active;
                        command.Parameters.Add("@CPId", SqlDbType.Int).Value = carePlan.CPId;

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            // Should also send an error message to user later or take to "oh no" page.
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            return RedirectToAction("PatientCarePlan", new { mhn = carePlan.MHN });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult CreatePatientNotesForm(int mhn)
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
        public IActionResult CreatePatientNotesForm(PatientNotes patientNote)
        {

            // Needed to work with the patient banner properly.
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = _listService.GetPatientByMHN(patientNote.MHN);
            ViewBag.Patient = viewModel.PatientDemographic;
            viewModel.PatientNotesDetails = patientNote;
            ViewBag.MHN = patientNote.MHN;


            if (patientNote.visitsId == -1)
            {
                ModelState.AddModelError("PatientNotesDetails.visitsId", "Please select a visit.");
            }

            if (patientNote.associatedProvider == -1) 
            {
                ModelState.AddModelError("PatientNotesDetails.associatedProvider", "Please select an associated provider.");
            }

            if (patientNote.createdBy == -1)
            {
                ModelState.AddModelError("PatientNotesDetails.createdBy", "Please select who is creating the note.");
            }

            if (patientNote.occurredOn <= DateOnly.FromDateTime(DateTime.Now.AddYears(-5)))
            {
                ModelState.AddModelError("PatientNotesDetails.occurredOn", "The date cannot be more than 5 years in the past");
            }
            else if (patientNote.occurredOn > DateOnly.FromDateTime(DateTime.Now))
            {
                ModelState.AddModelError("PatientNotesDetails.occurredOn", "The date cannot be in the future");
            }

            if (!Regex.IsMatch(patientNote.category, @"^[a-zA-Z\s]+$"))
            {
                ModelState.AddModelError("PatientNotesDetails.category", "Category must only contain letters and spaces.");
            }

            if (patientNote.Note == null)
            {
                ModelState.AddModelError("PatientNotesDetails.Note", "Please enter a note.");
            }
            else if (!Regex.IsMatch(patientNote.Note, @"^[a-zA-Z0-9\s.,'""!?()\-]*$"))
            {
                ModelState.AddModelError("PatientNotesDetails.Note", "Note must only contain letters, numbers, and punctuation.");
            }


            // Don't need to check for these since they aren't on the form.
            ModelState.Remove("visits");
            ModelState.Remove("patients");
            ModelState.Remove("providers");
            ModelState.Remove("assocProvider");

            // Returns the model if there are validation errors.
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    //SQL query that is going to insert the data that the user entered into the database table.
                    string sql = "INSERT INTO [PatientNotes] (MHN, note, occurredOn, createdAt, createdBy, associatedProvider, updatedAt, category, visitsId) " +
                        "VALUES (@mhn, @note, @occurredOn, @createdAt, @createdBy, @associatedProvider, @updatedAt, @category, @visitsId)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;

                        //adding parameters
                        command.Parameters.Add("@mhn", SqlDbType.Int).Value = patientNote.MHN;
                        command.Parameters.Add("@note", SqlDbType.VarChar).Value = patientNote.Note;
                        command.Parameters.Add("@occurredOn", SqlDbType.Date).Value = patientNote.occurredOn;
                        command.Parameters.Add("@createdAt", SqlDbType.DateTime2).Value = DateTime.Now;
                        command.Parameters.Add("@createdBy", SqlDbType.Int).Value = patientNote.createdBy;
                        command.Parameters.Add("@associatedProvider", SqlDbType.Int).Value = patientNote.associatedProvider;
                        command.Parameters.Add("@updatedAt", SqlDbType.DateTime2).Value = DateTime.Now;
                        command.Parameters.Add("@category", SqlDbType.VarChar).Value = patientNote.category;
                        command.Parameters.Add("@visitsId", SqlDbType.Int).Value = patientNote.visitsId;


                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            // Should also send an error message to user later or take to "oh no" page.
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            return RedirectToAction("PatientNotes", new { mhn = patientNote.MHN });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult EditPatientNotesForm(int noteId)
        {
            // Needed to work with the patient banner properly.
            PortalViewModel viewModel = new PortalViewModel();

            PatientNotes note = new PatientNotes();


            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                // Sql query to get the patient with the passed in mhn.
                string sql = "SELECT MHN, Note, occurredOn, createdAt, createdBy, associatedProvider, updatedAt, category, visitsId " +
                    "FROM [dbo].[PatientNotes] WHERE patientNotesId = @notesId";

                SqlCommand cmd = new SqlCommand(sql, connection);

                // Replace placeholder with paramater to avoid sql injection.
                cmd.Parameters.AddWithValue("@notesId", noteId);

                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        note.patientNotesId = noteId;
                        note.MHN = Convert.ToInt32(dataReader["MHN"]);
                        note.Note = Convert.ToString(dataReader["Note"]);

                        note.occurredOn = DateOnly.FromDateTime(Convert.ToDateTime(dataReader["occurredOn"]));

                        note.createdAt = DateTime.Parse(dataReader["createdAt"].ToString());
                        note.createdBy = Convert.ToInt32(dataReader["createdBy"]);
                        note.associatedProvider = Convert.ToInt32(dataReader["associatedProvider"]);
                        note.updatedAt = DateTime.Parse(dataReader["updatedAt"].ToString());
                        note.category = Convert.ToString(dataReader["category"]);
                        note.visitsId = Convert.ToInt32(dataReader["visitsId"]);
                    }
                }

                connection.Close();
            }

            viewModel.PatientNotesDetails = note;
            viewModel.PatientDemographic = _listService.GetPatientByMHN(note.MHN);
            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = viewModel.PatientNotesDetails.MHN;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult EditPatientNotesForm(PatientNotes patientNote)
        {

            // Needed to work with the patient banner properly.
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = _listService.GetPatientByMHN(patientNote.MHN);
            ViewBag.Patient = viewModel.PatientDemographic;
            viewModel.PatientNotesDetails = patientNote;
            ViewBag.MHN = patientNote.MHN;


            if (patientNote.visitsId == -1)
            {
                ModelState.AddModelError("PatientNotesDetails.visitsId", "Please select a visit.");
            }

            if (patientNote.associatedProvider == -1)
            {
                ModelState.AddModelError("PatientNotesDetails.associatedProvider", "Please select an associated provider.");
            }

            if (patientNote.createdBy == -1)
            {
                ModelState.AddModelError("PatientNotesDetails.createdBy", "Please select who is creating the note.");
            }

            if (patientNote.occurredOn <= DateOnly.FromDateTime(DateTime.Now.AddYears(-5)))
            {
                ModelState.AddModelError("PatientNotesDetails.occurredOn", "The date cannot be more than 5 years in the past");
            }
            // Makes sure the occurred on date doesn't get changed to after the date it was created.
            else if (patientNote.occurredOn > new DateOnly(patientNote.createdAt.Year, patientNote.createdAt.Month, patientNote.createdAt.Day))
            {
                ModelState.AddModelError("PatientNotesDetails.occurredOn", "The occurrence date cannot be after the note creation date.");
            }

            if (!Regex.IsMatch(patientNote.category, @"^[a-zA-Z\s]+$"))
            {
                ModelState.AddModelError("PatientNotesDetails.category", "Category must only contain letters and spaces.");
            }

            if (patientNote.Note == null)
            {
                ModelState.AddModelError("PatientNotesDetails.Note", "Note must not be empty.");
            }
            else if (!Regex.IsMatch(patientNote.Note, @"^[a-zA-Z0-9\s.,'""!?()\-]*$"))
            {
                ModelState.AddModelError("PatientNotesDetails.Note", "Note must only contain letters, numbers, and punctuation.");
            }

            // Don't need to check for these since they aren't on the form.
            ModelState.Remove("visits");
            ModelState.Remove("patients");
            ModelState.Remove("providers");
            ModelState.Remove("assocProvider");

            // Returns the model if there are validation errors.
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    //SQL query that is going to insert the data that the user entered into the database table.
                    string sql = "UPDATE [dbo].[PatientNotes] SET Note = @note, occurredOn = @occurredOn, createdBy = @createdBy, " +
                                 "associatedProvider = @associatedProvider, updatedAt = @updatedAt, category = @category, visitsId = @visitsId " +
                                 "WHERE patientNotesId = @notesId";


                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;

                        //adding parameters
                        command.Parameters.Add("@notesId", SqlDbType.Int).Value = patientNote.patientNotesId;
                        command.Parameters.Add("@note", SqlDbType.VarChar).Value = patientNote.Note;
                        command.Parameters.Add("@occurredOn", SqlDbType.Date).Value = patientNote.occurredOn;
                        //command.Parameters.Add("@createdAt", SqlDbType.DateTime2).Value = DateTime.Now;
                        command.Parameters.Add("@createdBy", SqlDbType.Int).Value = patientNote.createdBy;
                        command.Parameters.Add("@associatedProvider", SqlDbType.Int).Value = patientNote.associatedProvider;
                        command.Parameters.Add("@updatedAt", SqlDbType.DateTime2).Value = DateTime.Now;
                        command.Parameters.Add("@category", SqlDbType.VarChar).Value = patientNote.category;
                        command.Parameters.Add("@visitsId", SqlDbType.Int).Value = patientNote.visitsId;


                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            // Should also send an error message to user later or take to "oh no" page.
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            return RedirectToAction("PatientNotes", new { mhn = patientNote.MHN });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CreateAllergyForm(int mhn)
        {
            // Needed to work with the patient banner properly.
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = _listService.GetPatientByMHN(mhn);

            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = mhn;

            return View(viewModel);
        }

        // still testing below
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateAllergyForm(PatientAllergies allergy)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            DateOnly pastLimit = today.AddYears(-20);

            // Validate onsetDate
            if (allergy.onSetDate == DateOnly.MinValue)
            {
                ModelState.AddModelError("PatientAllergy.onSetDate", "Please enter allergy onset date.");
            }
            else if (allergy.onSetDate > today)
            {
                ModelState.AddModelError("PatientAllergy.onSetDate", "Onset date cannot be in the future.");
            }
            else if (allergy.onSetDate < pastLimit)
            {
                ModelState.AddModelError("PatientAllergy.onSetDate", "Onset date cannot be more than 20 years ago.");
            }

            if (allergy.allergyId == 0)
            {
                ModelState.AddModelError("PatientAllergy.allergyId", "Please select an allergy.");
            }

            //returns the model if null because there were errors in validating it
            if (!ModelState.IsValid)
            {
                // Needed to work with the patient banner properly.
                PortalViewModel viewModel = new PortalViewModel();
                viewModel.PatientDemographic = _listService.GetPatientByMHN(allergy.MHN);
                viewModel.PatientAllergy = allergy;
                ViewBag.Patient = viewModel.PatientDemographic;
                ViewBag.MHN = allergy.MHN;

                return View(viewModel);
            }
            else if (allergy.MHN != 0)
            {
                //go to the void list service that will input the data into the database.
                _listService.InsertIntoPatientAllergies(allergy);
            }

            return RedirectToAction("PatientAllergies", new { mhn = allergy.MHN });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CreateAlertForm(int mhn)
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
        public IActionResult CreateAlertForm(Alerts alert)
        {
            DateTime today = DateTime.Today;
            DateTime pastLimit = today.AddYears(-5);

            if (ModelState.TryGetValue("Alert.startDate", out var startDateEntry) && startDateEntry.Errors.Any(e => e.ErrorMessage == "The value '' is invalid."))
            {
                ModelState.Remove("Alert.startDate");
                ModelState.AddModelError("Alert.startDate", "Please enter a valid start date.");
            }

            if (ModelState.TryGetValue("Alert.endDate", out var endDateEntry) && endDateEntry.Errors.Any(e => e.ErrorMessage == "The value '' is invalid."))
            {
                ModelState.Remove("Alert.endDate");
                ModelState.AddModelError("Alert.endDate", "Please enter a valid end date.");
            }

            else if (alert.startDate > today)
            {
                ModelState.AddModelError("Alert.startDate", "Start date cannot be in the future.");
            }
            else if (alert.startDate < pastLimit)
            {
                ModelState.AddModelError("Alert.startDate", "Start date cannot be more than 5 years ago.");
            }
            else if (alert.endDate < alert.startDate)
            {
                ModelState.Remove("Alert.endDate");
                ModelState.AddModelError("Alert.endDate", "End date cannot be before start date.");
            }
            else if (alert.alertName == null)
            {
                ModelState.AddModelError("Alert.alertName", "Please enter alert name.");
            }

            ModelState.Remove("alert.patients");

            //returns the model if null because there were errors in validating it
            if (!ModelState.IsValid)
            {
                // Needed to work with the patient banner properly.
                PortalViewModel viewModel = new PortalViewModel();
                viewModel.PatientDemographic = _listService.GetPatientByMHN(alert.MHN);
                //viewModel.Alerts.Add(alert);
                viewModel.Alert = alert;
                ViewBag.Patient = viewModel.PatientDemographic;
                ViewBag.MHN = alert.MHN;

                return View(viewModel);
            }
            else if (alert.MHN != 0)
            {
                //go to the void list service that will input the data into the database.
                _listService.InsertIntoAlerts(alert);
                _listService.UpdateHasAlerts(alert.MHN);
            }

            return RedirectToAction("PatientAlerts", new { mhn = alert.MHN });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult EditAllergyForm(int patientAllergyId)
        {
            // Needed to work with the patient banner properly.
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientAllergy = _listService.GetPatientAllergyByPatientAllergyId(patientAllergyId);
            viewModel.PatientDemographic = _listService.GetPatientByMHN(viewModel.PatientAllergy.MHN);

            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = viewModel.PatientAllergy.MHN;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult EditAllergyForm(PatientAllergies allergy)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            DateOnly pastLimit = today.AddYears(-20);

            // validate onset date
            if (allergy.onSetDate == DateOnly.MinValue)
            {
                ModelState.AddModelError("PatientAllergy.onSetDate", "Please enter allergy onset date.");
            }
            else if (allergy.onSetDate > today)
            {
                ModelState.AddModelError("PatientAllergy.onSetDate", "Onset date cannot be in the future.");
            }
            else if (allergy.onSetDate < pastLimit)
            {
                ModelState.AddModelError("PatientAllergy.onSetDate", "Onset date cannot be more than 20 years ago.");
            }
        
            if (allergy.allergyId <= 0)
            {
                ModelState.AddModelError("PatientAllergy.allergyId", "Please select an allergy.");
            }

            //returns the model if null because there were errors in validating it
            if (!ModelState.IsValid)
            {
                // Needed to work with the patient banner properly.
                PortalViewModel viewModel = new PortalViewModel();
                viewModel.PatientDemographic = _listService.GetPatientByMHN(allergy.MHN);
                viewModel.PatientAllergy = allergy;
                ViewBag.Patient = viewModel.PatientDemographic;
                ViewBag.MHN = allergy.MHN;

                return View("EditAllergyForm", viewModel);
            }
            else if (allergy.MHN != 0)
            {
                //go to the void list service that will input the data into the database.
                _listService.UpdatePatientAllergy(allergy);
            }

            return RedirectToAction("PatientAllergies", new { mhn = allergy.MHN });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult EditAlertForm(int alertId)
        {
            // Needed to work with the patient banner properly.
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.Alert = _listService.GetPatientAlert(alertId);
            viewModel.PatientDemographic = _listService.GetPatientByMHN(viewModel.Alert.MHN);

            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = viewModel.Alert.MHN;
            ViewBag.AlertId = alertId;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult EditAlertForm(Alerts alert)
        {
            DateTime today = DateTime.Today;
            DateTime pastLimit = today.AddYears(-5);

            if (ModelState.TryGetValue("Alert.startDate", out var startDateEntry) && startDateEntry.Errors.Any(e => e.ErrorMessage == "The value '' is invalid."))
            {
                ModelState.Remove("Alert.startDate");
                ModelState.AddModelError("Alert.startDate", "Please enter a valid start date.");
            }

            if (ModelState.TryGetValue("Alert.endDate", out var endDateEntry) && endDateEntry.Errors.Any(e => e.ErrorMessage == "The value '' is invalid."))
            {
                ModelState.Remove("Alert.endDate");
                ModelState.AddModelError("Alert.endDate", "Please enter a valid end date.");
            }
            else if (alert.startDate > today)
            {
                ModelState.AddModelError("Alert.startDate", "Start date cannot be in the future.");
            }
            else if (alert.startDate < pastLimit)
            {
                ModelState.AddModelError("Alert.startDate", "Start date cannot be more than 5 years ago.");
            }
            else if (alert.endDate < alert.startDate)
            {
                ModelState.Remove("Alert.endDate");
                ModelState.AddModelError("Alert.endDate", "End date cannot be before start date.");
            }
            else if (alert.alertName == null)
            {
                ModelState.AddModelError("Alert.alertName", "Please enter alert name.");
            }

            ModelState.Remove("alert.patients");

            //returns the model if null because there were errors in validating it
            if (!ModelState.IsValid)
            {
                // Needed to work with the patient banner properly.
                PortalViewModel viewModel = new PortalViewModel();
                viewModel.PatientDemographic = _listService.GetPatientByMHN(alert.MHN);
                viewModel.Alert = alert;
                ViewBag.Patient = viewModel.PatientDemographic;
                ViewBag.MHN = alert.MHN;

                return View(viewModel);
            }
            else if (alert.MHN != 0)
            {
                //go to the void list service that will input the data into the database.
                _listService.UpdatePatientAlert(alert);
            }
            

            return RedirectToAction("PatientAlerts", new { mhn = alert.MHN });         
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditProfilePicture(IFormFile file, int mhn)
        {
            if (file == null || file.Length == 0)
            {
                return Json(new { success = false, message = "No file uploaded." });
            }

            if (file.Length > 4 * 1024 * 1024) // File size check
            {
                return Json(new { success = false, message = "File size must be less than 4MB." });
            }

            var allowedFileTypes = new[] { ".jpg", ".png" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedFileTypes.Contains(extension))
            {
                return Json(new { success = false, message = "Invalid file type. Only .jpg or .png allowed." });
            }

            var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            var fileName = Guid.NewGuid().ToString() + extension;
            var filePath = Path.Combine(uploadDirectory, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Update patient record with new file path
            PatientDemographic patient = _listService.GetPatientByMHN(mhn);
            patient.patientImage = fileName;
            _listService.UpdatePatientImage(patient);

            return Json(new { success = true, message = "File uploaded successfully", filePath = $"/images/{fileName}" });
        }

        // delete patient allergy record
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult DeletePatientAllergy(int patientAllergyId)
        {

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                string sql = "DELETE FROM [PatientAllergies] WHERE patientAllergyId = @patientAllergyId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@patientAllergyId", SqlDbType.Int).Value = patientAllergyId;

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        connection.Close();

                        if (rowsAffected <= 0)
                        {
                            throw new Exception(patientAllergyId + " not found.");
                        }
                        else
                        {
                            return Ok("Successfully deleted allergy.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.ToString());
                        return BadRequest("Failed to delete allergy");
                    }
                }
            }
        }        

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult DeletePatientCarePlan(int carePlanId)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                string sql = "DELETE FROM [CarePlan] WHERE CPId = @carePlanId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@carePlanId", SqlDbType.Int).Value = carePlanId;

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        connection.Close();

                        if (rowsAffected <= 0)
                        {
                            throw new Exception(carePlanId + " not found.");
                        }
                        else
                        {
                            return Ok("Successfully deleted insurance.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.ToString());
                        return BadRequest("Failed to delete insurance");
                    }
                }
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult DeletePatientNote(int noteId)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                string sql = "DELETE FROM [PatientNotes] WHERE patientNotesId = @noteId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@noteId", SqlDbType.Int).Value = noteId;

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        connection.Close();

                        if (rowsAffected <= 0)
                        {
                            throw new Exception(noteId + " not found.");
                        }
                        else
                        {
                            return Ok("Successfully deleted insurance.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.ToString());
                        return BadRequest("Failed to delete insurance");
                    }
                }
            }
        }

        // delete patient alert record, change hasAlerts status if there are no alerts left for that patient
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteAlert(int alertId)
        {
            int mhn = _listService.GetPatientFromAlert(alertId);
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                string sql = "DELETE FROM [Alerts] WHERE alertId = @alertId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@alertId", SqlDbType.Int).Value = alertId;

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        connection.Close();

                        if (rowsAffected <= 0)
                        {
                            throw new Exception(alertId + " not found.");
                        }
                        else
                        {   
                            bool hasAlerts = _listService.CheckPatientAlerts(mhn);
                            if (!hasAlerts)
                            {
                                _listService.DeleteHasAlerts(mhn);
                            }
                            return Ok("Successfully deleted alert.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.ToString());
                        return BadRequest("Failed to delete alert");
                    }
                }
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult DeletePatientInsurance(int insuranceId)
        {

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                string sql = "DELETE FROM [PatientInsurance] WHERE patientInsuranceId = @insuranceId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@insuranceId", SqlDbType.Int).Value = insuranceId;

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        connection.Close();

                        if (rowsAffected <= 0)
                        {
                            throw new Exception( insuranceId + " not found.");
                        }
                        else
                        {
                            return Ok("Successfully deleted insurance.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.ToString());
                        return BadRequest("Failed to delete insurance");
                    }
                }
            }
        }

        [HttpPost]
        [Route("Patient/DeleteVitals")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteVitals(int vitalsId)
        {

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                string sql = "DELETE FROM [Vitals] WHERE vitalsId = @vitalsId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@vitalsId", SqlDbType.Int).Value = vitalsId;

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        connection.Close();

                        if (rowsAffected <= 0)
                        {
                            throw new Exception(vitalsId + " not found.");
                        }
                        else
                        {
                            return Ok("Successfully deleted insurance.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.ToString());
                        return BadRequest("Failed to delete insurance");
                    }
                }
            }
        }
    }
}
