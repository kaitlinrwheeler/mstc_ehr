using EHRApplication.Models;
using EHRApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

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
    }
}
