using EHRApplication.Models;
using EHRApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.ComponentModel;
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
