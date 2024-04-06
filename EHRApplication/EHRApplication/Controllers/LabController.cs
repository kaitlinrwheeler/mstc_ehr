using EHRApplication.Models;
using EHRApplication.Services;
using EHRApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace EHRApplication.Controllers
{
    public class LabController : BaseController
    {
        public LabController(ILogService logService, IListService listService, IConfiguration configuration)
            :base(logService, listService, configuration)
        {
        }

        public IActionResult LabOrders(int mhn)
        {
            //This will set the banner up and the view model so we can view everything
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = _listService.GetPatientByMHN(mhn);

            //Calls the list service to get all of the Lab orders associated to the passed in mhn number.
            List<LabOrders> labOrders = _listService.GetPatientsLabOrdersByMHN(mhn);

            //This will add the patient lab orders to the view model so it can be displayed along with the banner at the same time.
            viewModel.LabOrders = labOrders;

            //This will add all of the data to a view bag the will be grabbed else where to display data correctly.
            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.LabOrders = viewModel.LabOrders;
            ViewBag.MHN = mhn;

            return View(viewModel);
        }

        public IActionResult AllLabTests()
        {
            // New list to hold all the tests in the database.
            List<LabTestProfile> allTests = new List<LabTestProfile>();

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                // Sql query.
                string sql = "SELECT testId, testName, description, units, referenceRange, category, Active FROM [dbo].[LabTestProfile] ORDER BY Active ASC";

                SqlCommand cmd = new SqlCommand(sql, connection);

                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        // Create a new test object for each record.
                        LabTestProfile test = new LabTestProfile();

                        // Populate the test object with data from the database.
                        test.testId = Convert.ToInt32(dataReader["testId"]);
                        test.testName = Convert.ToString(dataReader["testName"]);
                        test.description = Convert.ToString(dataReader["description"]);
                        test.units = Convert.ToString(dataReader["units"]);
                        test.referenceRange = Convert.ToString(dataReader["referenceRange"]);
                        test.category = Convert.ToString(dataReader["category"]);
                        test.Active = Convert.ToBoolean(dataReader["Active"]);

                        // Add the test to the list
                        allTests.Add(test);
                    }
                }

                connection.Close();
            }

            // Return the view with the list of all the patients so we can display them.
            return View(allTests);
        }

        [HttpPost]
        public IActionResult UpdateLabActiveStatus(int id, bool activeStatus)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                // Sql query.
                string sql = "UPDATE [dbo].[LabTestProfile] SET Active = @active WHERE testId = @id";

                SqlCommand cmd = new SqlCommand(sql, connection);

                // Replace placeholder with parameter to avoid SQL injection.
                cmd.Parameters.AddWithValue("@mhn", id);
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

    }
}
