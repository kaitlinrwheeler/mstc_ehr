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

        public IEnumerable<MedicationProfile> GetMedicationProfiles()
        {
            // New list to hold all the patients in the database.
            List<MedicationProfile> medsList = new List<MedicationProfile>();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                // Sql query.
                string sql = "SELECT * FROM [dbo].[MedicationProfile] ORDER BY medId ASC";

                SqlCommand cmd = new SqlCommand(sql, connection);

                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        // Create a new patient object for each record.
                        MedicationProfile medication = new MedicationProfile();

                        // Populate the medication object with data from the database.
                        medication.medId = Convert.ToInt32(dataReader["medId"]);
                        medication.medName = Convert.ToString(dataReader["medName"]);
                        medication.description = Convert.ToString(dataReader["description"]);
                        medication.route = Convert.ToString(dataReader["route"]);

                        // Add the patient to the list
                        medsList.Add(medication);
                    }
                }

                connection.Close();
            }
            return medsList;
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
        public PatientContact GetContactByMHN(int mhn)
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

        /// <summary>
        /// Gets the contact info from the database for that specific patient
        /// </summary>
        /// <param name="mhn"></param>
        /// <returns></returns>
        public MedicationProfile GetMedicationProfileByMedId(int medId)
        {
            // New medication profile instance to hold the medicaiton profile for the selected user.
            MedicationProfile medicationProfile = new MedicationProfile();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                // Sql query.
                string sql = "SELECT * FROM [dbo].[MedicationProfile] WHERE medId = @medId ORDER BY medId ASC";

                SqlCommand cmd = new SqlCommand(sql, connection);

                // Replace placeholder with paramater to avoid sql injection.
                cmd.Parameters.AddWithValue("@medId", medId);
                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        // Populate the medication object with data from the database.
                        medicationProfile.medId = Convert.ToInt32(dataReader["medId"]);
                        medicationProfile.medName = Convert.ToString(dataReader["medName"]);
                        medicationProfile.description = Convert.ToString(dataReader["description"]);
                        medicationProfile.route = Convert.ToString(dataReader["route"]);
                    }
                }

                connection.Close();
            }
            return medicationProfile;
        }

        /// <summary>
        /// Gets the allergy object from the database for that specific allergy id.
        /// </summary>
        /// <param name="allergyId"></param>
        /// <returns>The allergy object</returns>
        public Allergies GetAllergyByAllergyId(int allergyId)
        {
            //Creating a new instance of the allergy class to store data from the database
            Allergies allergy = new Allergies();

            //Setting up the connection with the database
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                //SQL command to select the data from the table
                string sql = "Select * From [dbo].[Allergies] WHERE AllergyId = @allergyId";
                SqlCommand cmd = new SqlCommand(sql, connection);

                // Replace placeholder with paramater to avoid sql injection.
                cmd.Parameters.AddWithValue("@allergyId", allergyId);
                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        //Setting the data that was just pulled from the database into an instance of the allergies model.
                        allergy.allergyId = allergyId;
                        allergy.allergyName = Convert.ToString(dataReader["AllergyName"]);
                        allergy.allergyType = Convert.ToString(dataReader["AllergyType"]);
                    }
                };
                connection.Close();
                return allergy;
            }
        }

    }
}
