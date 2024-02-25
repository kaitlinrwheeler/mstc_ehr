using EHRApplication.Models;
using EHRApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace EHRApplication.Controllers
{
    public class LabTestProfileController : Controller
    {
        private readonly LogService _logService;

        //database connection string
        private readonly string _connectionString;

        public LabTestProfileController(LogService logService, IConfiguration configuration)
        {
            _logService = logService;
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }

        /// <summary>
        /// Displays the view that shows all the lab tests from the database
        /// </summary>
        /// <returns></returns>
        public IActionResult AllLabTests()
        {
            //list for lab tests
            List<LabTestProfile> allTests = new List<LabTestProfile>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                //query to grab the testId, testName, description, units, referenceRange, category from LabTestProfile table
                string sql = "SELECT testId, testName, description, units, referenceRange, category FROM [dbo].[LabTestProfile] ORDER BY testId ASC";

                SqlCommand cmd = new SqlCommand(sql, connection);

                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        LabTestProfile labTest = new LabTestProfile
                        {
                            testId = Convert.ToInt32(dataReader["testId"]),
                            testName = Convert.ToString(dataReader["testName"]),
                            description = Convert.ToString(dataReader["description"]),
                            units = Convert.ToString(dataReader["units"]),
                            referenceRange = Convert.ToString(dataReader["referenceRange"]),
                            category = Convert.ToString(dataReader["category"])
                        };

                        allTests.Add(labTest);
                    }
                }

                connection.Close();
            }

            return View(allTests);
        }
    }
}
