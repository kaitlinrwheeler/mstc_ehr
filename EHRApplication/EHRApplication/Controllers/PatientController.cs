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

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult PatientOverView()
        {
            //Creating a new patientDemographic instance
            PatientDemographic patientDemographic = new PatientDemographic();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                //Grabbing data from the database using the listService
                var providersList = new ListService(Configuration).GetProviders();
                var contactList = new ListService(Configuration).GetContacts();

                connection.Open();
                //Making a sql command
                //SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[PatientDemographic] WHERE MHN = @MHN", connection);
                string sql = "SELECT * FROM [dbo].[PatientDemographic] WHERE MHN = 1";
                SqlCommand cmd = new SqlCommand(sql, connection);

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
                        patientDemographic.ContactId = contactList.Where(type => type.MHN == patientDemographic.MHN).FirstOrDefault();
                    }
                }

                connection.Close();
            }
            return View(patientDemographic);
        }
    }
}
