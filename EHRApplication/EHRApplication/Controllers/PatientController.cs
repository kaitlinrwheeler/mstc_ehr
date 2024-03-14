using EHRApplication.Models;
using EHRApplication.Services;
using EHRApplication.ViewModels;
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
                string sql = "SELECT MHN, firstName, lastName, DOB, gender FROM [dbo].[PatientDemographic] ORDER BY MHN ASC";

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

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                //SQL query that is going to insert the data that the user entered into the database table.
                string sql = "INSERT INTO [PatientDemographic] (firstName, middleName, lastName, suffix, preferredPronouns, DOB, gender, preferredLanguage, ethnicity, race, religion, primaryPhysician, legalGuardian1, legalGuardian2, genderAssignedAtBirth) " +
                    "VALUES (@firstName, @middleName, @lastName, @suffix, @preferredPronouns, @DOB, @gender, @preferredLanguage, @ethnicity, @race, @religion, @primaryPhysician, @legalGuardian1, @legalGuardian2, @genderAssignedAtBirth)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;

                    patient.race = string.Join(",", patient.raceList);

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
                    command.Parameters.Add("@genderAssignedAtBirth", SqlDbType.VarChar).Value = patient.genderAssignedAtBirth;

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
            List<Visits> patientVisits = new List<Visits>();

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                // Sql query.
                string sql = "SELECT providerId, date, time, admitted, notes FROM [dbo].[Visits] WHERE MHN = @mhn ORDER BY date DESC";

                SqlCommand cmd = new SqlCommand(sql, connection);

                // Replace placeholder with paramater to avoid sql injection.
                cmd.Parameters.AddWithValue("@mhn", mhn);

                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        // Create a new patient object for each record.
                        Visits visit = new Visits();

                        //Populate the visits object with data from the database.
                        visit.providerId = Convert.ToInt32(dataReader["providerId"]);
                        //Gets the provider for this patient using the primary physician number that links to the providers table
                        visit.providers = _listService.GetProvidersByProviderId(visit.providerId);
                        //This is grabbing the date from the database and converting it to date only. Somehow even though it is 
                        //Saved to the database as only a date it does not read as just a date so this converts it to dateOnly.
                        DateTime dateTime = DateTime.Parse(dataReader["date"].ToString());
                        visit.date = new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
                        visit.time = TimeOnly.Parse(dataReader["time"].ToString());
                        visit.admitted = Convert.ToBoolean(dataReader["admitted"]);
                        visit.notes = Convert.ToString(dataReader["notes"]);

                        // Add the patient to the list
                        patientVisits.Add(visit);
                    }
                }

                viewModel.Visits = patientVisits;
                ViewBag.Patient = viewModel.PatientDemographic;
                ViewBag.MHN = mhn;
                connection.Close();
            }
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
                        allergy.allergies = new ListService(this._connectionString).GetAllergyByAllergyId(allergy.allergyId);




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
    }
}
