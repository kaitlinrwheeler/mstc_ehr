using EHRApplication.Models;
using EHRApplication.Services;
using EHRApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Configuration;
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
            viewModel.PatientDemographic = GetPatientByMHN(mhn);

            // List to hold the patient's list of problems.
            List<PatientProblems> problems = new List<PatientProblems>();

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                // Sql query.
                string sql = "SELECT * FROM [dbo].[PatientProblems] WHERE MHN = @mhn ORDER BY MHN ASC";

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

        private PatientDemographic GetPatientByMHN(int mhn)
        {
            //Creating a new patientDemographic instance
            PatientDemographic patientDemographic = new PatientDemographic();

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
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
                        //This is grabbing the date of birth from the database and converting it to date only. Somehow even though it is 
                        //Saved to the database as only a date it does not read as just a date so this converts it to dateOnly.
                        DateTime dateTime = DateTime.Parse(dataReader["DOB"].ToString());
                        patientDemographic.DOB = new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
                        patientDemographic.gender = Convert.ToString(dataReader["gender"]);
                        patientDemographic.preferredLanguage = Convert.ToString(dataReader["preferredLanguage"]);
                        patientDemographic.ethnicity = Convert.ToString(dataReader["ethnicity"]);
                        patientDemographic.race = Convert.ToString(dataReader["race"]);
                        patientDemographic.religion = Convert.ToString(dataReader["religion"]);
                        patientDemographic.primaryPhysician = Convert.ToInt32(dataReader["primaryPhysician"]);
                        //Gets the provider for this patient using the primary physician number that links to the providers table
                        patientDemographic.providers = _listService.GetProvidersByProviderId(patientDemographic.primaryPhysician);
                        patientDemographic.legalGuardian1 = Convert.ToString(dataReader["legalGuardian1"]);
                        patientDemographic.legalGuardian2 = Convert.ToString(dataReader["legalGuardian2"]);
                        patientDemographic.previousName = Convert.ToString(dataReader["previousName"]);
                        //Gets the contact info for this patient using the MHN that links to the contact info table
                        patientDemographic.genderAssignedAtBirth = Convert.ToString(dataReader["genderAssignedAtBirth"]);
                        patientDemographic.ContactId = _listService.GetContactByMHN(patientDemographic.MHN);
                    }
                }

                connection.Close();
            }

            return patientDemographic;
        }
    }
}
