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
                string sql = "SELECT providerId, firstName, lastName, specialty FROM [dbo].[Providers] ORDER BY providerId ASC";

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
                            specialty = Convert.ToString(dataReader["specialty"])
                        };

                        allProviders.Add(provider);
                    }
                }

                connection.Close();
            }

            return View(allProviders);
        }

        public IActionResult CreateProvider(int providerId)
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

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "INSERT INTO [Providers] (firstName, lastName, specialty) VALUES (@firstName, @lastName, @specialty)";

                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@firstName", provider.firstName);
                    cmd.Parameters.AddWithValue("@lastName", provider.lastName);
                    cmd.Parameters.AddWithValue("@specialty", provider.specialty);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return View();
        }

        public IActionResult EditProvider(int providerId)
        {
            Providers provider = new Providers();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sql = "SELECT providerId, firstName, lastName, specialty FROM [dbo].[Providers] WHERE providerId = @providerId";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@providerId", providerId);

                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        provider.providerId = Convert.ToInt32(dataReader["providerId"]);
                        provider.firstName = Convert.ToString(dataReader["firstName"]);
                        provider.lastName = Convert.ToString(dataReader["lastName"]);
                        provider.specialty = Convert.ToString(dataReader["specialty"]);
                    }
                }

                connection.Close();
            }

            return View(provider);
        }

        [HttpPost]
        public IActionResult UpdateProviderActiveStatus(int providerId, bool activeStatus)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                // Sql query.
                string sql = "UPDATE [dbo].[Providers] SET Active = @active WHERE providerId = @providerId";

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
