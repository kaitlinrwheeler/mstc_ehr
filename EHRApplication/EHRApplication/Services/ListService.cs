using EHRApplication.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EHRApplication.Services
{
    public class ListService : IListService
    {
        private readonly string connectionString;
        public IConfiguration Configuration { get; }

        public ListService(IConfiguration configuration)
        {
            Configuration = configuration;
            connectionString = Configuration["ConnectionStrings:DefaultConnection"];
        }

        public IEnumerable<Providers> GetProviders()
        {
            List<Providers> providerList = new List<Providers>();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                DataTable dataTable = new DataTable();

                string sql = "Select * From Providers";
                SqlCommand cmd = new SqlCommand(sql, connection);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                adapter.Fill(dataTable);

                foreach (DataRow row in dataTable.Rows)
                {
                    providerList.Add(
                        new Providers
                        {
                            providerId = Convert.ToInt32(row["providerId"]),
                            firstName = Convert.ToString(row["firstName"]),
                            lastName = Convert.ToString(row["lastName"]),
                            specialty = Convert.ToString(row["specialty"]),
                        });
                }
                return providerList;
            }
        }

        public IEnumerable<PatientContact> GetContacts()
        {
            List<PatientContact> contactList = new List<PatientContact>();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                DataTable dataTable = new DataTable();

                string sql = "SELECT * FROM [dbo].[PatientContact] WHERE MHN = 1";
                SqlCommand cmd = new SqlCommand(sql, connection);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                adapter.Fill(dataTable);

                foreach (DataRow row in dataTable.Rows)
                {
                    contactList.Add(
                        new PatientContact
                        {
                            MHN = Convert.ToInt32(row["MHN"]),
                            address = Convert.ToString(row["address"]),
                            city = Convert.ToString(row["city"]),
                            state = Convert.ToString(row["state"]),
                            zipcode = Convert.ToInt32(row["zipcode"]),
                            phone = Convert.ToString(row["phone"]),
                            email = Convert.ToString(row["email"]),
                        });
                }
                return contactList;
            }
        }
    }
}
