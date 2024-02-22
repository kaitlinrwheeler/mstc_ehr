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

        /// <summary>
        /// Gets a list of all the providers from the database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Providers> GetProviders()
        {
            //creates a new instance of the providers model as a list
            List<Providers> providerList = new List<Providers>();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                //Table that will be a tempory holder of the database untill we can convert it into a list
                DataTable dataTable = new DataTable();

                //SQL query that selects everything and sorts it in asc order
                string sql = "Select * From [dbo].[Providers] ORDER BY providerId ASC";
                SqlCommand cmd = new SqlCommand(sql, connection);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                adapter.Fill(dataTable);

                //loops through all of the providers that were pulled and adds them to the list after setting the individual properties
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

        /// <summary>
        /// Gets a list of all the contacts from the database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PatientContact> GetContacts()
        {
            //Creates a new instance of the patient contact model as a list
            List<PatientContact> contactList = new List<PatientContact>();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                //Table that will be a tempory holder of the database untill we can convert it into a list
                DataTable dataTable = new DataTable();

                //SQL query that selects everything from the patient contact table and sorts it in asc order
                string sql = "SELECT * FROM [dbo].[PatientContact] ORDER BY MHN ASC";
                SqlCommand cmd = new SqlCommand(sql, connection);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                adapter.Fill(dataTable);

                //loops through all of the contact info that was pulled and adds them to the list after setting the individual properties
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
                            ECFirstName = Convert.ToString(row["ECFirstName"]),
                            ECLastName = Convert.ToString(row["ECLastName"]),
                            ECRelationship = Convert.ToString(row["ECRelationship"]),
                            ECPhone = Convert.ToString(row["ECPhone"]),
                        });
                }
                return contactList;
            }
        }

        /// <summary>
        /// Gets the provider from the database for that specific patient
        /// </summary>
        /// <param name="providerId"></param>
        /// <returns></returns>
        public Providers GetProvidersByProviderId(int providerId)
        {
            //Creating a new instance of the patient contact class to store data from the database
            Providers providers = new Providers();

            //Setting up the connection with the database
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                //SQL command to select the data from the table
                string sql = "Select providerId, firstName, lastName, specialty From [dbo].[Providers] WHERE providerId = @providerId";
                SqlCommand cmd = new SqlCommand(sql, connection);

                // Replace placeholder with paramater to avoid sql injection.
                cmd.Parameters.AddWithValue("@providerId", providerId);
                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        //Setting the data that was just pulled from the database into an instance of the patient contact model.
                        providers.providerId = Convert.ToInt32(dataReader["providerId"]);
                        providers.firstName = Convert.ToString(dataReader["firstName"]);
                        providers.lastName = Convert.ToString(dataReader["lastName"]);
                        providers.specialty = Convert.ToString(dataReader["specialty"]);
                    }
                };
                connection.Close();
                return providers;
            }
        }

        /// <summary>
        /// Gets the contact info from the database for that specific patient
        /// </summary>
        /// <param name="mhn"></param>
        /// <returns></returns>
        public PatientContact GetContactsByMHN(int mhn)
        {
            //Creating a new instance of the patient contact class to store data from the database
            PatientContact patientContact = new PatientContact();

            //Setting up the connection with the database
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                //SQL command to select the data from the table
                string sql = "SELECT address, city, state, zipcode, phone, email  FROM [dbo].[PatientContact] WHERE MHN = @mhn";
                SqlCommand cmd = new SqlCommand(sql, connection);

                // Replace placeholder with paramater to avoid sql injection.
                cmd.Parameters.AddWithValue("@mhn", mhn);
                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        //Setting the data that was just pulled from the database into an instance of the patient contact model.
                        patientContact.address = Convert.ToString(dataReader["address"]);
                        patientContact.city = Convert.ToString(dataReader["city"]);
                        patientContact.state = Convert.ToString(dataReader["state"]);
                        patientContact.zipcode = Convert.ToInt32(dataReader["zipcode"]);
                        patientContact.phone = Convert.ToString(dataReader["phone"]);
                        patientContact.email = Convert.ToString(dataReader["email"]);
                    }
                };
                connection.Close();
                return patientContact;
            }
        }
    }
}
