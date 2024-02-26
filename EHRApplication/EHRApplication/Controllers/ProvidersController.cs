using EHRApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace EHRApplication.Controllers
{
    public class ProvidersController : Controller
    {
        private readonly LogService _logService;

        //database connection string
        private readonly string _connectionString;

        public ProvidersController(LogService logService, IConfiguration configuration)
        {
            _logService = logService;
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
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
    }
}
