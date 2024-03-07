using EHRApplication.Models;
using EHRApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace EHRApplication.Controllers
{
    public class MedicationsController : Controller
    {
        private readonly LogService _logService;
        //Setting the default connection string
        private string connectionString;
        public IConfiguration Configuration { get; }

        public MedicationsController(LogService logService, IConfiguration configuration)
        {
            _logService = logService;
            Configuration = configuration;
            this.connectionString = Configuration["ConnectionStrings:DefaultConnection"];
        }

        /// <summary>
        /// Displays the view that shows all the medications in the database
        /// </summary>
        /// <returns></returns>
        public IActionResult AllMedications()
        {
            // New list to hold all the patients in the database.
            List<MedicationProfile> allMeds = new List<MedicationProfile>();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                // Sql query.
                string sql = "SELECT * FROM [dbo].[MedicationProfile] ORDER BY medId ASC";

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
            // New list to hold all the patients in the database.
            List<PatientMedications> patientMeds = new List<PatientMedications>();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
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
                        medication.MHN = Convert.ToInt32(dataReader["MHN"]);
                        medication.medId = Convert.ToInt32(dataReader["medId"]);
                        medication.medProfile = new ListService(Configuration).GetMedicationProfileByMedId(medication.medId);
                        medication.category = Convert.ToString(dataReader["category"]);
                        medication.activeStatus = Convert.ToString(dataReader["activeStatus"]);
                        medication.prescriptionInstructions = Convert.ToString(dataReader["prescriptionInstructions"]);
                        medication.dosage = Convert.ToString(dataReader["dosage"]);
                        medication.route = Convert.ToString(dataReader["route"]);
                        medication.prescribedBy = Convert.ToInt32(dataReader["prescribedBy"]);
                        //Gets the provider for this patient using the primary physician number that links to the providers table
                        medication.providers = new ListService(Configuration).GetProvidersByProviderId(medication.prescribedBy);
                        medication.datePrescribed = Convert.ToDateTime(dataReader["datePrescribed"]);
                        medication.endDate = Convert.ToDateTime(dataReader["endDate"]);

                            // Add the patient to the list
                        patientMeds.Add(medication);
                    }
                }

                connection.Close();
            }
            return View(patientMeds);
        }

        public IActionResult PatientMedHistory(int mhn)
        {
            //Calls the list service to get all of the med history associated to the passed in mhn number.
            List<MedAdministrationHistory> patientHistory = new ListService(Configuration).GetPatientsMedHistoryByMHN(mhn);
            return View(patientHistory);
        }
    }
}