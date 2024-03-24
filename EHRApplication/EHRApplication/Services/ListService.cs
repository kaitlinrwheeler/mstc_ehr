using EHRApplication.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EHRApplication.Services
{
    public class ListService : IListService
    {
        private readonly string _connectionString;

        public ListService(string connectionString)
        {
            this._connectionString = connectionString;
        }

        /// <summary>
        /// Gets a list of all the providers from the database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Providers> GetProviders()
        {
            //creates a new instance of the providers model as a list
            List<Providers> providerList = new List<Providers>();

            using (SqlConnection connection = new SqlConnection(this._connectionString))
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

            using (SqlConnection connection = new SqlConnection(this._connectionString))
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
        /// Gets a list of all the providers from the database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PatientDemographic> GetPatients()
        {
            //creates a new instance of the providers model as a list
            List<PatientDemographic> patientList = new List<PatientDemographic>();

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                //Table that will be a tempory holder of the database untill we can convert it into a list
                DataTable dataTable = new DataTable();

                //SQL query that selects everything and sorts it in asc order
                string sql = "Select * From [dbo].[PatientDemographic] ORDER BY MHN ASC";
                SqlCommand cmd = new SqlCommand(sql, connection);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                adapter.Fill(dataTable);

                //loops through all of the providers that were pulled and adds them to the list after setting the individual properties
                foreach (DataRow row in dataTable.Rows)
                {
                    //This is grabbing the date of birth from the table and saving it as a variable so that when we add it below we are able to add only the date.
                    DateTime dateTime = DateTime.Parse(row["DOB"].ToString());

                    patientList.Add(
                        new PatientDemographic
                        {
                            MHN = Convert.ToInt32(row["MHN"]),
                            firstName = Convert.ToString(row["firstName"]),
                            middleName = Convert.ToString(row["middleName"]),
                            lastName = Convert.ToString(row["lastName"]),
                            suffix = Convert.ToString(row["suffix"]),
                            preferredPronouns = Convert.ToString(row["preferredPronouns"]),
                            //This is taking the date that was grabbed up above and only setting the date to the DOB for the model
                            DOB = new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day),
                            gender = Convert.ToString(row["gender"]),
                            preferredLanguage = Convert.ToString(row["preferredLanguage"]),
                            ethnicity = Convert.ToString(row["ethnicity"]),
                            race = Convert.ToString(row["race"]),
                            religion = Convert.ToString(row["religion"]),
                            primaryPhysician = Convert.ToInt32(row["primaryPhysician"]),
                            legalGuardian1 = Convert.ToString(row["legalGuardian1"]),
                            legalGuardian2 = Convert.ToString(row["legalGuardian2"]),
                            previousName = Convert.ToString(row["previousName"]),
                            genderAssignedAtBirth = Convert.ToString(row["genderAssignedAtBirth"])
                        });
                }
                return patientList;
            }
        }

        public IEnumerable<MedicationProfile> GetMedicationProfiles()
        {
            // New list to hold all the patients in the database.
            List<MedicationProfile> medsList = new List<MedicationProfile>();

            using (SqlConnection connection = new SqlConnection(this._connectionString))
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
            using (SqlConnection connection = new SqlConnection(this._connectionString))
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

        public LabTestProfile GetLabTestByTestId(int testId)
        {
            //Creating a new instance of the patient contact class to store data from the database
            LabTestProfile labTest = new LabTestProfile();

            //Setting up the connection with the database
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();
                //SQL command to select the data from the table
                string sql = "Select * From [dbo].[LabTestProfile] WHERE testId = @testId";
                SqlCommand cmd = new SqlCommand(sql, connection);

                // Replace placeholder with paramater to avoid sql injection.
                cmd.Parameters.AddWithValue("@testId", testId);
                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        //Setting the data that was just pulled from the database into an instance of the patient contact model.
                        labTest.testId = Convert.ToInt32(dataReader["testId"]);
                        labTest.testName = Convert.ToString(dataReader["testName"]);
                        labTest.description = Convert.ToString(dataReader["description"]);
                        labTest.units = Convert.ToString(dataReader["units"]);
                        labTest.referenceRange = Convert.ToString(dataReader["referenceRange"]);
                        labTest.category = Convert.ToString(dataReader["category"]);
                    }
                };
                connection.Close();
                return labTest;
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
            using (SqlConnection connection = new SqlConnection(this._connectionString))
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

            using (SqlConnection connection = new SqlConnection(this._connectionString))
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
            using (SqlConnection connection = new SqlConnection(this._connectionString))
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

        public List<MedAdministrationHistory> GetPatientsMedHistoryByMHN(int mhn)
        {
            //Create a new instance of the Med History class to store data from the database
            List<MedAdministrationHistory> historyList = new List<MedAdministrationHistory>();
            var patients = GetPatients();

            //Setting up the connection with the database
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();
                //SQL command to select the data from the table
                string sql = "Select * From [dbo].[MedAdministrationHistory] WHERE MHN = @mhn ORDER By dateGiven DESC, timeGiven DESC";
                SqlCommand cmd = new SqlCommand(sql, connection);

                //Replace placeholder with paramater to avoid sql injection.
                cmd.Parameters.AddWithValue("@MHN", mhn);
                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        //Setting the data that was just pulled from the database into an instance of the med history model.
                        MedAdministrationHistory medHistory = new MedAdministrationHistory();

                        // Populate the medHistory object with data from the database.
                        medHistory.administrationId = Convert.ToInt32(dataReader["administrationId"]);

                        //Gets the MHN from the table and then uses that to grab the patient associated to the history and
                        //Creates a object that is saved to the med history object.
                        medHistory.MHN = Convert.ToInt32(dataReader["MHN"]);
                        medHistory.patients = patients.Where(patients => patients.MHN == mhn).FirstOrDefault();
                        
                        medHistory.category = Convert.ToString(dataReader["category"]);
                        medHistory.medId = Convert.ToInt32(dataReader["medId"]);
                        medHistory.medProfile = GetMedicationProfileByMedId(medHistory.medId);
                        medHistory.status = Convert.ToString(dataReader["status"]);
                        medHistory.frequency = Convert.ToString(dataReader["frequency"]);
                        //This is grabbing the date from the database and converting it to date only. Somehow even though it is 
                        //Saved to the database as only a date it does not read as just a date so this converts it to dateOnly.
                        DateTime dateTime = DateTime.Parse(dataReader["dateGiven"].ToString());
                        medHistory.dateGiven = new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
                        medHistory.timeGiven = TimeOnly.Parse(dataReader["timeGiven"].ToString());

                        //Gets the provider id from the table and then uses that to grab the provider associated to it.
                        //Creates an object that is saved to the med history object.
                        medHistory.administeredBy = Convert.ToInt32(dataReader["administeredBy"]);
                        medHistory.providers = GetProvidersByProviderId(medHistory.administeredBy);

                        // Add the patient to the list
                        historyList.Add(medHistory);
                    }
                }
                connection.Close();
            }
            return historyList;
        }

        public List<LabResults> GetPatientsLabResultsByMHN(int mhn)
        {
            //Create a new instance of the Med History class to store data from the database
            List<LabResults> labResultsList = new List<LabResults>();

            //When we move the get patient by MHN into the service use that one instead.
            var patients = GetPatients();

            //Setting up the connection with the database
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();
                //SQL command to select the data from the table
                string sql = "Select * From [dbo].[LabResults] WHERE MHN = @mhn ORDER By date DESC, time DESC";
                SqlCommand cmd = new SqlCommand(sql, connection);

                //Replace placeholder with paramater to avoid sql injection.
                cmd.Parameters.AddWithValue("@MHN", mhn);
                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        //Setting the data that was just pulled from the database into an instance of the med history model.
                        LabResults labResults = new LabResults();

                        // Populate the medHistory object with data from the database.
                        labResults.labId = Convert.ToInt32(dataReader["labId"]);

                        //Gets the MHN from the table and then uses that to grab the patient associated to the history and
                        //Creates a object that is saved to the med history object.
                        labResults.MHN = Convert.ToInt32(dataReader["MHN"]);
                        labResults.patients = patients.Where(patients => patients.MHN == mhn).FirstOrDefault();

                        labResults.visitsId = Convert.ToInt32(dataReader["visitsId"]);
                        labResults.visits = GetPatientVisitsByMHN(mhn).FirstOrDefault();

                        labResults.testId = Convert.ToInt32(dataReader["testId"]);
                        labResults.labTests = GetLabTestByTestId(labResults.testId);

                        labResults.resultValue = Convert.ToString(dataReader["resultValue"]);
                        labResults.abnormalFlag = Convert.ToString(dataReader["abnormalFlag"]);

                        labResults.orderedBy = Convert.ToInt32(dataReader["orderedBy"]);
                        labResults.providers = GetProvidersByProviderId(labResults.orderedBy);

                        //This is grabbing the date from the database and converting it to date only. Somehow even though it is 
                        //Saved to the database as only a date it does not read as just a date so this converts it to dateOnly.
                        DateTime dateTime = DateTime.Parse(dataReader["date"].ToString());
                        labResults.date = new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
                        labResults.time = TimeOnly.Parse(dataReader["time"].ToString());

                        // Add the patient to the list
                        labResultsList.Add(labResults);
                    }
                }
                connection.Close();
            }
            return labResultsList;
        }

        public List<Visits> GetPatientVisitsByMHN(int mhn)
        {
            List<Visits> patientVisits = new List<Visits>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Sql query.
                string sql = "SELECT * FROM [dbo].[Visits] WHERE MHN = @mhn ORDER BY date DESC";

                SqlCommand cmd = new SqlCommand(sql, connection);

                // Replace placeholder with paramater to avoid sql injection.
                cmd.Parameters.AddWithValue("@mhn", mhn);

                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        // Create a new patient object for each record.
                        Visits visit = new Visits();

                        visit.visitsId = Convert.ToInt32(dataReader["visitsId"]);
                        //Populate the visits object with data from the database.
                        visit.providerId = Convert.ToInt32(dataReader["providerId"]);
                        //Gets the provider for this patient using the primary physician number that links to the providers table
                        visit.providers = new ListService(_connectionString).GetProvidersByProviderId(visit.providerId);
                        //This is grabbing the date from the database and converting it to date only. Somehow even though it is 
                        //Saved to the database as only a date it does not read as just a date so this converts it to dateOnly.
                        DateTime dateTime = DateTime.Parse(dataReader["date"].ToString());
                        visit.date = new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
                        visit.time = TimeOnly.Parse(dataReader["time"].ToString());
                        visit.admitted = Convert.ToBoolean(dataReader["admitted"]);
                        visit.notes = Convert.ToString(dataReader["notes"]);


                        // Add the patient to the list
                        patientVisits.Add(visit);
                    }
                }
                connection.Close();
            }
            return patientVisits;
        }


        public List<CarePlan> GetCarePlanByMHN(int mhn)
        {
            //List that will hold all of the care plans for the patient with the passed in mhn number.
            List<CarePlan> carePlanList = new List<CarePlan>();

            //Setting up the connection with the database
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                //SQL command to select the data from the database
                string sql = "Select * From [dbo].[CarePlan] WHERE MHN = @mhn ORDER BY CASE WHEN activeStatus = 'active' THEN 1 ELSE 2 END, startDate DESC";
                SqlCommand cmd = new SqlCommand(sql, connection);

                //Replace placeholder with paramater to avoid sql injection.
                cmd.Parameters.AddWithValue("@mhn", mhn);
                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        //Creating a new instance of the care plan class to store data form the database
                        CarePlan carePlan = new CarePlan();

                        //Setting the data that was jus pulled from the database into an instance of the care plan model.
                        carePlan.priority = Convert.ToString(dataReader["priority"]);
                        carePlan.activeStatus = Convert.ToString(dataReader["activeStatus"]);
                        carePlan.title = Convert.ToString(dataReader["title"]);
                        carePlan.diagnosis = Convert.ToString(dataReader["diagnosis"]);
                        carePlan.startDate = Convert.ToDateTime(dataReader["startDate"]);
                        carePlan.endDate = Convert.ToDateTime(dataReader["endDate"]);

                        //After setting the data pulled from the database now adding it to the list that will be returned.
                        carePlanList.Add(carePlan);
                    }
                }
                connection.Close();
                return carePlanList;
            }
        }

        /// <summary>
        /// Gets the visit from the database that matches the visit id.
        /// </summary>
        /// <param name="visitId"></param>
        /// <returns></returns>
        public Visits GetVisitByVisitId(int visitId)
        {
            //Creating a new instance of the patient contact class to store data from the database
            Visits visit = new Visits();

            //Setting up the connection with the database
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();
                //SQL command to select the data from the table
                string sql = "Select * From [dbo].[Visits] WHERE visitsId = @visitId";
                SqlCommand cmd = new SqlCommand(sql, connection);

                // Replace placeholder with paramater to avoid sql injection.
                cmd.Parameters.AddWithValue("@visitId", visitId);
                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        visit.visitsId = dataReader.GetInt32("visitsId");
                        //Setting the data that was just pulled from the database into an instance of the visit model.
                        visit.MHN = dataReader.GetInt32("MHN");
                        visit.date = DateOnly.FromDateTime(dataReader.GetDateTime(dataReader.GetOrdinal("date")));

                        TimeSpan timeSpan = dataReader.GetTimeSpan(dataReader.GetOrdinal("time"));
                        visit.time = new TimeOnly(timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

                        visit.admitted = dataReader.GetBoolean("admitted");
                        visit.notes = dataReader.GetString("notes");

                        //Populate the visits object with data from the database.
                        visit.providerId = Convert.ToInt32(dataReader["providerId"]);
                        //Gets the provider for this patient using the primary physician number that links to the providers table
                        visit.providers = GetProvidersByProviderId(visit.providerId);
                    }
                };
                connection.Close();
                return visit;
            }
        }


        public List<LabOrders> GetPatientsLabOrdersByMHN(int mhn)
        {
            //Create a new instance of the Med History class to store data from the database
            List<LabOrders> labOrdersList = new List<LabOrders>();

            //Setting up the connection with the database
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();
                //SQL command to select the data from the table
                string sql = "Select * From [dbo].[LabOrders] WHERE MHN = @mhn ORDER By orderDate DESC, orderTime DESC";
                SqlCommand cmd = new SqlCommand(sql, connection);

                //Replace placeholder with paramater to avoid sql injection.
                cmd.Parameters.AddWithValue("@MHN", mhn);
                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        //Setting the data that was just pulled from the database into an instance of the med history model.
                        LabOrders labOrders = new LabOrders();

                        // Populate the labOrders object with data from the database.
                        labOrders.orderId = Convert.ToInt32(dataReader["orderId"]);

                        //Gets the MHN from the table and then uses that to grab the patient associated to the order and
                        //Creates a object that is saved to the lab order object.
                        labOrders.MHN = Convert.ToInt32(dataReader["MHN"]);
                        labOrders.patients = GetPatientByMHN(mhn);
                        
                        //Gets the test id from the table and then uses that to grab the patient associated to the order and
                        //Creates a object that is saved to the lab order object.
                        labOrders.testId = Convert.ToInt32(dataReader["testId"]);
                        labOrders.labTests = GetLabTestByTestId(labOrders.testId);

                        labOrders.visitsId = Convert.ToInt32(dataReader["visitsId"]);
                        //labOrders.visits = GetPatientVisitsByMHN(labOrders.visitsId);

                        labOrders.completionStatus = Convert.ToString(dataReader["completionStatus"]);

                        //This is grabbing the date from the database and converting it to date only. Somehow even though it is 
                        //Saved to the database as only a date it does not read as just a date so this converts it to dateOnly.
                        DateTime dateTime = DateTime.Parse(dataReader["orderDate"].ToString());
                        labOrders.orderDate = new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
                        labOrders.orderTime = TimeOnly.Parse(dataReader["orderTime"].ToString());

                        //Gets the provider id from the table and then uses that to grab the provider associated to it.
                        //Creates an object that is saved to the med history object.
                        labOrders.orderedBy = Convert.ToInt32(dataReader["orderedBy"]);
                        labOrders.providers = GetProvidersByProviderId(labOrders.orderedBy);

                        // Add the patient to the list
                        labOrdersList.Add(labOrders);
                    }
                }
                connection.Close();
            }
            return labOrdersList;
        }

        public PatientDemographic GetPatientByMHN(int mhn)
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
                        patientDemographic.providers = GetProvidersByProviderId(patientDemographic.primaryPhysician);
                        patientDemographic.legalGuardian1 = Convert.ToString(dataReader["legalGuardian1"]);
                        patientDemographic.legalGuardian2 = Convert.ToString(dataReader["legalGuardian2"]);
                        patientDemographic.previousName = Convert.ToString(dataReader["previousName"]);
                        //Gets the contact info for this patient using the MHN that links to the contact info table
                        patientDemographic.genderAssignedAtBirth = Convert.ToString(dataReader["genderAssignedAtBirth"]);
                        patientDemographic.ContactId = GetContactByMHN(patientDemographic.MHN);
                    }
                }

                connection.Close();
            }

            return patientDemographic;
        }
    }
}
