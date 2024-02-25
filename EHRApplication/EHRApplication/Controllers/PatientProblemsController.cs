using EHRApplication.Models;
using EHRApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace EHRApplication.Controllers
{
    public class ProblemsController : Controller
    {

        private readonly LogService _logService;

        private string connectionString;
        public IConfiguration Configuration { get; }

        public ProblemsController(LogService logService, IConfiguration configuration)
        {
            _logService = logService;
            Configuration = configuration;
            this.connectionString = Configuration["ConnectionStrings:DefaultConnection"];
        }


        public IActionResult Index(int mhn)
        {
            // List to hold the patient's list of problems.
            List<PatientProblems> problems = new List<PatientProblems>();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
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
                        problem.providers = new ListService(Configuration).GetProvidersByProviderId(problem.createdBy);
                        problem.active = Convert.ToBoolean(dataReader["active"]);

                        // Add the patient to the list
                        problems.Add(problem);
                    }
                }

                connection.Close();
            }

            return View(problems);
        }
    }
}
