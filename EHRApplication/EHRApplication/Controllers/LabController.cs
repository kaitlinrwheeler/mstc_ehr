using EHRApplication.Models;
using EHRApplication.Services;
using EHRApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace EHRApplication.Controllers
{
    public class LabController : BaseController
    {
        public LabController(ILogService logService, IListService listService, IConfiguration configuration)
            : base(logService, listService, configuration)
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

        public IActionResult PatientLabResults(int mhn)
        {
            //This will set the banner up and the view model so we can view everything
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = _listService.GetPatientByMHN(mhn);

            //Calls the list service to get all of the Lab Results associated to the passed in mhn number.
            List<LabResults> patientHistory = _listService.GetPatientsLabResultsByMHN(mhn);

            //This will add the patient Lab Results to the view model so it can be displayed along with the banner at the same time.
            viewModel.LabResults = patientHistory;

            //This will add all of the data to a view bag the will be grabbed else where to display data correctly.
            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = mhn;

            return View(viewModel);
        }

        public IActionResult CreateOrderForm(int mhn)
        {
            // Needed to work with the patient banner properly.
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = _listService.GetPatientByMHN(mhn);

            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = mhn;

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult CreateOrderForm(LabOrders labOrder)
        {
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = _listService.GetPatientByMHN(labOrder.MHN);
            viewModel.LabOrdersDetails = labOrder;
            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = labOrder.MHN;

            //These test to make sure the select box has a value selected.
            if (labOrder.orderedBy == 0)
            {
                ModelState.AddModelError("LabOrdersDetails.orderedBy", "Please select a provider.");
            }            
            if (labOrder.testId == 0)
            {
                ModelState.AddModelError("LabOrdersDetails.testId", "Please select a test.");
            }            
            if (labOrder.visitsId == 0)
            {
                ModelState.AddModelError("LabOrdersDetails.visitsId", "Please select a visit.");
            }
            if (labOrder.completionStatus == "0")
            {
                ModelState.AddModelError("LabOrdersDetails.completionStatus", "Please select a status for the order.");
            }

            // Calculate the date one year ago from today
            DateTime oneYearAgo = DateTime.Now.AddYears(-1);
            // Testing to see if the date of birth entered is before 1920 or not
            if (labOrder.orderDate < DateOnly.FromDateTime(oneYearAgo))
            {
                // Adding an error to the date model to display an error.
                ModelState.AddModelError("LabOrdersDetails.orderDate", "Cannot set order for over a year ago.");
                return View(viewModel);
            }
            //returns the model if null because there were errors in validating it
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            else if (labOrder.MHN != 0)
            {
                //go to the void list service that will input the data into the database.
                _listService.InsertIntoLabOrders(labOrder);
            }

            return RedirectToAction("LabOrders", new { mhn = labOrder.MHN });
        }

        public IActionResult EditOrderForm(int orderId)
        {
            // Needed to work with the patient banner properly.
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.LabOrdersDetails = _listService.GetLabOrderByOrderId(orderId);
            viewModel.PatientDemographic = _listService.GetPatientByMHN(viewModel.LabOrdersDetails.MHN);

            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = viewModel.LabOrdersDetails.MHN;

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult EditOrderForm(LabOrders labOrder)
        {
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = _listService.GetPatientByMHN(labOrder.MHN);
            viewModel.LabOrdersDetails = labOrder;
            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = labOrder.MHN;

            //These test to make sure the select box has a value selected.
            if (labOrder.orderedBy == 0)
            {
                ModelState.AddModelError("LabOrdersDetails.orderedBy", "Please select a provider.");
            }
            if (labOrder.testId == 0)
            {
                ModelState.AddModelError("LabOrdersDetails.testId", "Please select a test.");
            }
            if (labOrder.visitsId == 0)
            {
                ModelState.AddModelError("LabOrdersDetails.visitsId", "Please select a visit.");
            }
            if (labOrder.completionStatus == "0")
            {
                ModelState.AddModelError("LabOrdersDetails.completionStatus", "Please select a status for the order.");
            }

            // Calculate the date one year ago from today
            DateTime oneYearAgo = DateTime.Now.AddYears(-1);
            // Testing to see if the date of birth entered is before 1920 or not
            if (labOrder.orderDate < DateOnly.FromDateTime(oneYearAgo))
            {
                // Adding an error to the date model to display an error.
                ModelState.AddModelError("LabOrdersDetails.orderDate", "Cannot set order for over a year ago.");
                return View(viewModel);
            }
            //returns the model if null because there were errors in validating it
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            else if (labOrder.MHN != 0)
            {
                //go to the void list service that will input the data into the database.
                _listService.UpdateLabOrders(labOrder);
            }

            return RedirectToAction("LabOrders", new { mhn = labOrder.MHN });
        }

        public IActionResult CreateResultsForm(int mhn)
        {
            // Needed to work with the patient banner properly.
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = _listService.GetPatientByMHN(mhn);

            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = mhn;

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult CreateResultsForm(LabResults labResult)
        {
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = _listService.GetPatientByMHN(labResult.MHN);
            viewModel.LabResultsDetails = labResult;
            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = labResult.MHN;

            //These test to make sure the select box has a value selected.
            if (labResult.orderedBy == 0)
            {
                ModelState.AddModelError("LabResultsDetails.orderedBy", "Please select a provider.");
            }
            if (labResult.testId == 0)
            {
                ModelState.AddModelError("LabResultsDetails.testId", "Please select a test.");
            }
            if (labResult.visitsId == 0)
            {
                ModelState.AddModelError("LabResultsDetails.visitsId", "Please select a visit.");
            }
            if (labResult.abnormalFlag == "0")
            {
                ModelState.AddModelError("LabResultsDetails.abnormalFlag", "Please select a value for the flag.");
            }
            if (labResult.resultValue.IsNullOrEmpty())
            {
                ModelState.AddModelError("LabResultsDetails.resultValue", "Please enter a result value.");
            }

            // Calculate the date one year ago from today
            DateTime oneYearAgo = DateTime.Now.AddYears(-1);
            // Testing to see if the date of birth entered is before 1920 or not
            if (labResult.date < DateOnly.FromDateTime(oneYearAgo))
            {
                // Adding an error to the date model to display an error.
                ModelState.AddModelError("LabResultsDetails.date", "Cannot set order for over a year ago.");
                return View(viewModel);
            }
            //returns the model if null because there were errors in validating it
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            if (labResult.MHN != 0)
            {
                //go to the void list service that will input the data into the database.
                _listService.InsertIntoLabResults(labResult);
            }

            return RedirectToAction("PatientLabResults", new { mhn = labResult.MHN });
        }

        public IActionResult EditResultsForm(int labId)
        {
            // Needed to work with the patient banner properly.
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.LabResultsDetails = _listService.GetLabResultByLabId(labId);
            viewModel.PatientDemographic = _listService.GetPatientByMHN(viewModel.LabResultsDetails.MHN);

            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = viewModel.LabResultsDetails.MHN;

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult EditResultsForm(LabResults labResult)
        {
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = _listService.GetPatientByMHN(labResult.MHN);
            viewModel.LabResultsDetails = labResult;
            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = labResult.MHN;

            //These test to make sure the select box has a value selected.
            if (labResult.orderedBy == 0)
            {
                ModelState.AddModelError("LabResultsDetails.orderedBy", "Please select a provider.");
            }
            if (labResult.testId == 0)
            {
                ModelState.AddModelError("LabResultsDetails.testId", "Please select a test.");
            }
            if (labResult.visitsId == 0)
            {
                ModelState.AddModelError("LabResultsDetails.visitsId", "Please select a visit.");
            }
            if (labResult.abnormalFlag == "0")
            {
                ModelState.AddModelError("LabResultsDetails.abnormalFlag", "Please select a value for the flag.");
            }
            if (labResult.resultValue.IsNullOrEmpty())
            {
                ModelState.AddModelError("LabResultsDetails.resultValue", "Please enter a result value.");
            }

            // Calculate the date one year ago from today
            DateTime oneYearAgo = DateTime.Now.AddYears(-1);
            // Testing to see if the date of birth entered is before 1920 or not
            if (labResult.date < DateOnly.FromDateTime(oneYearAgo))
            {
                // Adding an error to the date model to display an error.
                ModelState.AddModelError("LabResultsDetails.date", "Cannot set order for over a year ago.");
                return View(viewModel);
            }
            //returns the model if null because there were errors in validating it
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            if (labResult.MHN != 0)
            {
                //go to the void list service that will input the data into the database.
                _listService.UpdateLabResults(labResult);
            }

            return RedirectToAction("PatientLabResults", new { mhn = labResult.MHN });
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
                cmd.Parameters.AddWithValue("@Id", id);
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
