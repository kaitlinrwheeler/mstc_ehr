using EHRApplication.Models;
using EHRApplication.Services;
using EHRApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Net;

namespace EHRApplication.Controllers
{
    public class ProblemsController : BaseController
    {
        private readonly ILogService _logService;
        private readonly string _connectionString;
        private readonly IListService _listService;

        public ProblemsController(ILogService logService, IListService listService, IConfiguration configuration)
            :base(logService, listService, configuration)
        {
            _logService = logService;
            this._connectionString = base._connectionString;
            _listService = listService;
    }


        public IActionResult Index(int mhn)
        {
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = _listService.GetPatientByMHN(mhn);

            // List to hold the patient's list of problems.
            List<PatientProblems> problems = new List<PatientProblems>();

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                // Sql query.
                string sql = "SELECT * FROM [dbo].[PatientProblems] WHERE MHN = @mhn ORDER BY CASE WHEN active = 1 THEN 0 ELSE 1 END, MHN ASC";

                SqlCommand cmd = new SqlCommand(sql, connection);

                // Replace placeholder with paramater to avoid sql injection.
                cmd.Parameters.AddWithValue("@mhn", mhn);


                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        // Create a new problem object for each record.
                        PatientProblems problem = new PatientProblems();

                        // Populate the problem object with data from the database.

                        // I don't think we need to pull this one at all, but leaving this here for now.
                        problem.MHN = Convert.ToInt32(dataReader["MHN"]);

                        problem.patientProblemsId = Convert.ToInt32(dataReader["patientProblemsId"]);

                        problem.priority = Convert.ToString(dataReader["priority"]);
                        problem.description = Convert.ToString(dataReader["description"]);
                        problem.ICD_10 = Convert.ToString(dataReader["ICD_10"]);
                        problem.immediacy = Convert.ToString(dataReader["immediacy"]);
                        problem.createdAt = Convert.ToDateTime(dataReader["createdAt"]);
                        // Used only to get the actual provider in the next row down.
                        problem.createdBy = Convert.ToInt32(dataReader["createdBy"]);
                        problem.providers = _listService.GetProvidersByProviderId(problem.createdBy);
                        problem.active = Convert.ToBoolean(dataReader["active"]);

                        // Add the patient to the list
                        problems.Add(problem);
                    }
                }

                viewModel.PatientProblems = problems;
                ViewBag.Patient = viewModel.PatientDemographic;
                ViewBag.MHN = mhn;
                connection.Close();
            }

            return View(viewModel);
        }


        public IActionResult CreateProblemForm(int mhn)
        {
            // Needed to work with the patient banner properly.
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientDemographic = _listService.GetPatientByMHN(mhn);

            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = mhn;

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult CreateProblemForm(PatientProblems problem)
        {
            if (problem.visitsId == 0)
            {
                ModelState.AddModelError("PatientProblemsDetails.visitsId", "Please select a visit.");
            }
            if (problem.createdBy == 0)
            {
                ModelState.AddModelError("PatientProblemsDetails.createdBy", "Please select a provider.");
            }
            //returns the model if null because there were errors in validating it
            if (!ModelState.IsValid)
            {
                // Needed to work with the patient banner properly.
                PortalViewModel viewModel = new PortalViewModel();
                viewModel.PatientDemographic = _listService.GetPatientByMHN(problem.MHN);
                viewModel.PatientProblemsDetails = problem;
                ViewBag.Patient = viewModel.PatientDemographic;
                ViewBag.MHN = problem.MHN;

                return View(viewModel);
            }
            else if (problem.MHN != 0)
            {
                problem.createdAt = DateTime.Now;

                //go to the void list service that will input the data into the database.
                _listService.InsertIntoProblems(problem);
            }

            return RedirectToAction("index", new { mhn = problem.MHN });
        }

        public IActionResult EditProblemForm(int problemId)
        {
            // Needed to work with the patient banner properly.
            PortalViewModel viewModel = new PortalViewModel();
            viewModel.PatientProblemsDetails = _listService.GetPatientProblemsByProblemId(problemId);
            viewModel.PatientDemographic = _listService.GetPatientByMHN(viewModel.PatientProblemsDetails.MHN);

            ViewBag.Patient = viewModel.PatientDemographic;
            ViewBag.MHN = viewModel.PatientProblemsDetails.MHN;

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult EditProblemForm(PatientProblems problem)
        {
            if (problem.visitsId == 0)
            {
                ModelState.AddModelError("PatientProblemsDetails.visitsId", "Please select a visit.");
            }
            if (problem.createdBy == 0)
            {
                ModelState.AddModelError("PatientProblemsDetails.createdBy", "Please select a provider.");
            }
            //returns the model if null because there were errors in validating it
            if (!ModelState.IsValid)
            {
                // Needed to work with the patient banner properly.
                PortalViewModel viewModel = new PortalViewModel();
                viewModel.PatientDemographic = _listService.GetPatientByMHN(problem.MHN);
                viewModel.PatientProblemsDetails = problem;
                ViewBag.Patient = viewModel.PatientDemographic;
                ViewBag.MHN = problem.MHN;

                return View(viewModel);
            }
            else if (problem.MHN != 0)
            {
                problem.createdAt = DateTime.Now;

                //go to the void list service that will input the data into the database.
                _listService.UpdateProblems(problem);
            }

            return RedirectToAction("index", new { mhn = problem.MHN });
        }

        [HttpPost]
        [Route("Problems/DeletePatientProblem")]
        public IActionResult DeletePatientProblem(int problemId)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                string sql = "DELETE FROM [PatientProblems] WHERE patientProblemsId = @problemId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@problemId", SqlDbType.Int).Value = problemId;

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        connection.Close();

                        if (rowsAffected <= 0)
                        {
                            throw new Exception(problemId + " not found.");
                        }
                        else
                        {
                            return Ok("Successfully deleted insurance.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.ToString());
                        return BadRequest("Failed to delete insurance");
                    }
                }
            }
        }

    }
}
