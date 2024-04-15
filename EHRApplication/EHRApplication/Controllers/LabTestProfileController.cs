using EHRApplication.Models;
using EHRApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace EHRApplication.Controllers
{
    public class LabTestProfileController : BaseController
    {
        private readonly ILogService _logService;
        private readonly string _connectionString;
        private readonly IListService _listService;

        public LabTestProfileController(ILogService logService, IListService listService, IConfiguration configuration)
            : base(logService, listService, configuration)
        {
            _logService = logService;
            this._connectionString = base._connectionString;        
            _listService = listService;
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
                string sql = "SELECT testId, testName, description, units, referenceRange, category, Active FROM [dbo].[LabTestProfile] ORDER BY Active DESC";

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
                            category = Convert.ToString(dataReader["category"]),
                            Active = Convert.ToBoolean(dataReader["active"])
                        };

                        allTests.Add(labTest);
                    }
                }

                connection.Close();
            }

            return View(allTests);
        } 

        [HttpGet]
        public IActionResult CreateLabTest()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateLabTest(LabTestProfile labTest)
        {
            // We want to default to active when created.
            labTest.Active = true;

            if (!ModelState.IsValid)
            {
                return View(labTest);
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Insert a new record into the LabTestProfile table
                string sql = @"INSERT INTO [dbo].[LabTestProfile] 
                           (testName, description, units, referenceRange, category, Active)
                           VALUES 
                           (@testName, @description, @units, @referenceRange, @category, @active)";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@testName", labTest.testName);
                cmd.Parameters.AddWithValue("@description", labTest.description);
                cmd.Parameters.AddWithValue("@units", labTest.units);
                cmd.Parameters.AddWithValue("@referenceRange", labTest.referenceRange);
                cmd.Parameters.AddWithValue("@category", labTest.category);
                cmd.Parameters.AddWithValue("@active", labTest.Active);

                cmd.ExecuteNonQuery();

                connection.Close();
            }


            return RedirectToAction("AllLabTests");
        }

        [HttpGet]
        public IActionResult EditLabTest(int testId)
        {
            LabTestProfile labTest = new LabTestProfile();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                //query to grab the testId, testName, description, units, referenceRange, category from LabTestProfile table
                string sql = "SELECT testId, testName, description, units, referenceRange, category, Active FROM [dbo].[LabTestProfile] WHERE testId = @testId";

                SqlCommand cmd = new SqlCommand(sql, connection);

                cmd.Parameters.AddWithValue("@testId", testId);

                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        labTest.testId = Convert.ToInt32(dataReader["testId"]);
                        labTest.testName = Convert.ToString(dataReader["testName"]);
                        labTest.description = Convert.ToString(dataReader["description"]);
                        labTest.units = Convert.ToString(dataReader["units"]);
                        labTest.referenceRange = Convert.ToString(dataReader["referenceRange"]);
                        labTest.category = Convert.ToString(dataReader["category"]);
                        labTest.Active = Convert.ToBoolean(dataReader["active"]);
                    }
                }

                connection.Close();
            }

            return View(labTest);
        }


        [HttpPost]
        public IActionResult EditLabTest(LabTestProfile labTest)
        {
            // We want to default to active when edited.
            labTest.Active = true;

            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string sql = @"UPDATE [dbo].[LabTestProfile] 
                           SET testName = @testName, 
                               description = @description, 
                               units = @units, 
                               referenceRange = @referenceRange, 
                               category = @category, 
                               Active = @active
                           WHERE testId = @testId";

                    SqlCommand cmd = new SqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@testId", labTest.testId);
                    cmd.Parameters.AddWithValue("@testName", labTest.testName);
                    cmd.Parameters.AddWithValue("@description", labTest.description);
                    cmd.Parameters.AddWithValue("@units", labTest.units);
                    cmd.Parameters.AddWithValue("@referenceRange", labTest.referenceRange);
                    cmd.Parameters.AddWithValue("@category", labTest.category);
                    cmd.Parameters.AddWithValue("@active", labTest.Active);

                    cmd.ExecuteNonQuery();

                    connection.Close();
                }

                return RedirectToAction("AllLabTests");
            }

            // If the model state is not valid, return the view with errors
            return View(labTest);
        }


    }
}
