using EHRApplication.Models;
using EHRApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace EHRApplication.Controllers
{
    public class AllergyController : BaseController
    {
        private readonly ILogService _logService;
        private readonly IListService _listService;
        private readonly string _connectionString;

        public AllergyController(ILogService logService, IListService listService, IConfiguration configuration)
            : base(logService, listService, configuration)
        {
            _logService = logService;
            _listService = listService;
            _connectionString = base._connectionString;
        }

        public IActionResult Index()
        {
            // instantiate list of allergies
            List<Allergies> allAllergies = new List<Allergies>();

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                // SQL query
                string sql = "SELECT allergyId, allergyName, allergyType, activeStatus FROM [dbo].[Allergies] ORDER BY CASE WHEN activeStatus = 1 THEN 0 ELSE 1 END, allergyName ASC";

                SqlCommand cmd = new SqlCommand(sql, connection);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // create new allergy object
                        Allergies allergy = new Allergies();

                        // set allergy stats
                        allergy.allergyId = Convert.ToInt32(reader["allergyId"]);
                        allergy.allergyName = Convert.ToString(reader["allergyName"]);
                        allergy.allergyType = Convert.ToString(reader["allergyType"]);
                        allergy.activeStatus = Convert.ToBoolean(reader["activeStatus"]);

                        // add allergy list
                        allAllergies.Add(allergy);
                    }
                }

                connection.Close();
            }

            // return list to view
            return View(allAllergies);
        }


        [HttpPost]
        public IActionResult UpdateAllergyActiveStatus(int allergyId, bool activeStatus)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                // Sql query.
                string sql = "UPDATE [dbo].[Allergies] SET activeStatus = @active WHERE allergyId = @allergyId";

                SqlCommand cmd = new SqlCommand(sql, connection);

                // Replace placeholder with parameter to avoid SQL injection.
                cmd.Parameters.AddWithValue("@allergyId", allergyId);
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

        public IActionResult CreateAllergy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateAllergy(Allergies allergy)
        {            
            if (!ModelState.IsValid)
            {
                return View(allergy);
            }
            else
            {
                _listService.InsertIntoAllergies(allergy);
            }
            return RedirectToAction("Index");
        }

        public IActionResult EditAllergy(int allergyId)
        {
            Allergies allergy = _listService.GetAllergyById(allergyId);
            return View(allergy);
        }

        [HttpPost]
        public IActionResult EditAllergy(Allergies allergy)
        {
            if (!ModelState.IsValid)
            {
                return View(allergy);
            }
            else
            {
                _listService.UpdateAllergy(allergy);
            }
            return RedirectToAction("Index");
        }
    }
}
