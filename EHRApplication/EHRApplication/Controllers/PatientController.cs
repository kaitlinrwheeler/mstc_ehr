using EHRApplication.Models;
using EHRApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;

namespace EHRApplication.Controllers
{
    public class PatientController : Controller
    {
        private readonly LogService _logService;
        //Setting the default connection string
        private string connectionString;
        public IConfiguration Configuration { get; }

        public PatientController(LogService logService, IConfiguration configuration)
        {
            _logService = logService;
            Configuration = configuration;
            this.connectionString = Configuration["ConnectionStrings:DefaultConnection"];
        }

        public IActionResult AllPatients()
        {
            // New list to hold all the patients in the database.
            List<PatientDemographic> allPatients = new List<PatientDemographic>();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
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
                        patient.DOB = Convert.ToDateTime(dataReader["DOB"]);
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


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(PatientDemographic patient)
        {
            //returns the model if null because there were errors in validating it
            if (!ModelState.IsValid)
            {
                return View(patient);
            }

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                //SQL query that is going to insert the data that the user entered into the database table.
                string sql = "INSERT INTO [PatientDemographic] (firstName, middleName, lastName, suffix, preferredPronouns, DOB, gender, preferredLanguage, ethnicity, race, religion, primaryPhysician, legalGuardian1, legalGuardian2, genderAssignedAtBirth) " +
                    "VALUES (@firstName, @middleName, @lastName, @suffix, @preferredPronouns, @DOB, @gender, @preferredLanguage, @ethnicity, @race, @religion, @primaryPhysician, @legalGuardian1, @legalGuardian2, @genderAssignedAtBirth)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;

                    //adding parameters
                    command.Parameters.Add("@firstName", SqlDbType.VarChar).Value = patient.firstName;
                    command.Parameters.Add("@middleName", SqlDbType.VarChar).Value = patient.middleName;
                    command.Parameters.Add("@lastName", SqlDbType.VarChar).Value = patient.lastName;
                    command.Parameters.Add("@suffix", SqlDbType.VarChar).Value = patient.suffix;
                    command.Parameters.Add("@preferredPronouns", SqlDbType.VarChar).Value = patient.preferredPronouns;
                    command.Parameters.Add("@DOB", SqlDbType.Date).Value = patient.DOB;
                    command.Parameters.Add("@gender", SqlDbType.VarChar).Value = patient.gender;
                    command.Parameters.Add("@preferredLanguage", SqlDbType.VarChar).Value = patient.preferredLanguage;
                    command.Parameters.Add("@ethnicity", SqlDbType.VarChar).Value = patient.ethnicity;
                    command.Parameters.Add("@race", SqlDbType.VarChar).Value = patient.race;
                    command.Parameters.Add("@religion", SqlDbType.VarChar).Value = patient.religion;
                    command.Parameters.Add("@primaryPhysician", SqlDbType.VarChar).Value = patient.primaryPhysician;
                    command.Parameters.Add("@legalGuardian1", SqlDbType.VarChar).Value = patient.legalGuardian1;
                    command.Parameters.Add("@legalGuardian2", SqlDbType.VarChar).Value = patient.legalGuardian2;
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
        /// <summary>
        /// Trying to clear the validation state when the input box is changed
        /// </summary>
        /// <param name="validationName"></param>
        /// <returns></returns>
        public IActionResult ClearValidationState(string validationName)
        {
            // Clear validation state for the specified property
            ModelState.ClearValidationState(validationName);
            return RedirectToAction("Index");
        }

        public IActionResult PatientOverview(int mhn)
        {
            //Creating a new patientDemographic instance
            PatientDemographic patientDemographic = new PatientDemographic();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {

                // TODO: don't think this is right, should probably change this to do a query based off the mhn instead of getting the entire list?
                //Grabbing data from the database using the listService
                var providersList = new ListService(Configuration).GetProviders();
                var contactList = new ListService(Configuration).GetContacts();

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
                        patientDemographic.DOB = Convert.ToDateTime(dataReader["DOB"]);
                        patientDemographic.gender = Convert.ToString(dataReader["gender"]);
                        patientDemographic.preferredLanguage = Convert.ToString(dataReader["preferredLanguage"]);
                        patientDemographic.ethnicity = Convert.ToString(dataReader["ethnicity"]);
                        patientDemographic.race = Convert.ToString(dataReader["race"]);
                        patientDemographic.religion = Convert.ToString(dataReader["religion"]);
                        patientDemographic.primaryPhysician = Convert.ToInt32(dataReader["primaryPhysician"]);
                        patientDemographic.providers = providersList.Where(type => type.providerId == patientDemographic.primaryPhysician).FirstOrDefault();
                        patientDemographic.legalGuardian1 = Convert.ToString(dataReader["legalGuardian1"]);
                        patientDemographic.legalGuardian2 = Convert.ToString(dataReader["legalGuardian2"]);
                        patientDemographic.previousName = Convert.ToString(dataReader["previousName"]);
                        patientDemographic.genderAssignedAtBirth = Convert.ToString(dataReader["genderAssignedAtBirth"]);
                        patientDemographic.ContactId = contactList.Where(type => mhn == patientDemographic.MHN).FirstOrDefault();
                    }
                }

                connection.Close();
            }
            return View(patientDemographic);
        }
    }
}
