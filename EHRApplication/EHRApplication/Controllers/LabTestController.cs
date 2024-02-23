using EHRApplication.Models;
using EHRApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace EHRApplication.Controllers
{
    public class LabTestController : Controller
    {
        private readonly LogService _logService;
        //Setting the default connection string
        private string connectionString;
        public IConfiguration Configuration { get; }

        public LabTestController(LogService logService, IConfiguration configuration)
        {
            _logService = logService;
            Configuration = configuration;
            this.connectionString = Configuration["ConnectionStrings:DefaultConnection"];
        }

        /// <summary>
        /// Displays the view that shows all the lab tests in the database
        /// </summary>
        /// <returns></returns>
        public IActionResult AllLabTests()
        {
            // New list to hold all the lab tests in the database.
            List<LabTestProfile> allTests = new List<LabTestProfile>();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                // Sql query.
                string sql = "SELECT * FROM [dbo].[LabTestProfile] ORDER BY testId ASC";

                SqlCommand cmd = new SqlCommand(sql, connection);

                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        // Create a new lab test object for each record.
                        LabTestProfile labTest = new LabTestProfile();

                        // Populate the lab test object with data from the database.
                        labTest.testId = Convert.ToInt32(dataReader["testId"]);
                        labTest.testName = Convert.ToString(dataReader["testName"]);
                        labTest.description = Convert.ToString(dataReader["description"]);
                        labTest.units = Convert.ToString(dataReader["units"]);
                        labTest.referenceRange = Convert.ToString(dataReader["referenceRange"]);
                        labTest.category = Convert.ToString(dataReader["category"]);

                        // Add the test to the list
                        allTests.Add(labTest);
                    }
                }

                connection.Close();
            }

            // Return the view with the list of all the tests so we can display them.
            return View(allTests);
        }
    }
}
