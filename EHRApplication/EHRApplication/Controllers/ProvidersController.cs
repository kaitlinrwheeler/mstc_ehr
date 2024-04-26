using EHRApplication.Models;
using EHRApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EHRApplication.Controllers
{
    public class ProvidersController : BaseController
    {
        private readonly ILogService _logService;
        private readonly string _connectionString;
        private readonly IListService _listService;

        public ProvidersController(ILogService logService, IListService listService, IConfiguration configuration)
            : base(logService, listService, configuration)
        {
            _logService = logService;
            this._connectionString = base._connectionString;
            _listService = listService;
    }

        /// <summary>
        /// Displays the view that shows all the providers from the database
        /// </summary>
        /// <returns></returns>
        public IActionResult AllProviders()
        {
            //list for providers
            List<Providers> allProviders = new List<Providers>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                //query to grab the providerID, firstname, lastname, and specialty
                string sql = "SELECT providerId, firstName, lastName, specialty, active FROM [dbo].[Providers] ORDER BY CASE WHEN active = 1 THEN 0 ELSE 1 END, providerId ASC";

                SqlCommand cmd = new SqlCommand(sql, connection);

                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Providers provider = new Providers
                        {
                            providerId = Convert.ToInt32(dataReader["providerId"]),
                            firstName = Convert.ToString(dataReader["firstName"]),
                            lastName = Convert.ToString(dataReader["lastName"]),
                            specialty = Convert.ToString(dataReader["specialty"]),
                            active = Convert.ToBoolean(dataReader["active"])
                        };

                        allProviders.Add(provider);
                    }
                }

                connection.Close();
            }

            return View(allProviders);
        }

        public IActionResult CreateProvider()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateProvider(Providers provider)
        {
            if (!ModelState.IsValid)
            {
                return View(provider);
            }
            else
            {
                _listService.AddProvider(provider);
            }
            return RedirectToAction("AllProviders");
        }

        public IActionResult EditProvider(int providerId)
        {
            Providers provider = _listService.GetProviderById(providerId);
            return View(provider);
        }

        [HttpPost]
        public IActionResult EditProvider(Providers provider)
        {
            if (!ModelState.IsValid)
            {
                return View(provider);
            }
            else
            {
                _listService.UpdateProvider(provider);
            }

            return RedirectToAction("AllProviders");
        }

        [HttpPost]
        public IActionResult UpdateProviderActiveStatus(int providerId, bool activeStatus)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                // Sql query.
                string sql = "UPDATE [dbo].[Providers] SET active = @active WHERE providerId = @providerId";

                SqlCommand cmd = new SqlCommand(sql, connection);

                // Replace placeholder with parameter to avoid SQL injection.
                cmd.Parameters.AddWithValue("@providerId", providerId);
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
