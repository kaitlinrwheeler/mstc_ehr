using EHRApplication.Models;
using EHRApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EHRApplication.Controllers
{
    public class ContactController : Controller
    {
        private readonly LogService _logService;
        //Setting the default connection string
        private string connectionString;
        public IConfiguration Configuration { get; }

        public ContactController(LogService logService, IConfiguration configuration)
        {
            _logService = logService;
            Configuration = configuration;
            this.connectionString = Configuration["ConnectionStrings:DefaultConnection"];
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(PatientContact contact)
        {
            //returns the model if null because there were errors in validating it
            if (!ModelState.IsValid)
            {
                return View(contact);
            }

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                //SQL query that is going to insert the data that the user entered into the database table.
                string sql = "INSERT INTO [PatientContact] (MHN, address, city, state, zipcode, phone, email, ECFirstName, ECLastName, ECRelationship, ECPhone) " +
                    "VALUES (@MHN, @address, @city, @state, @zipcode, @phone, @email, @ECFirstName, @ECLastName, @ECRelationship, @ECPhone)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;


                    //The some of them test to see if the value if null or empty because they are optional on the form so if it is null or empty it will display NA otherwise will add the data enterd by the user.
                    //adding parameters
                    command.Parameters.Add("@MHN", SqlDbType.VarChar).Value = contact.MHN;
                    command.Parameters.Add("@address", SqlDbType.VarChar).Value = contact.address;
                    command.Parameters.Add("@city", SqlDbType.VarChar).Value = contact.city;
                    command.Parameters.Add("@state", SqlDbType.VarChar).Value = contact.state;
                    command.Parameters.Add("@zipcode", SqlDbType.VarChar).Value = contact.zipcode;
                    command.Parameters.Add("@phone", SqlDbType.VarChar).Value = contact.phone;
                    command.Parameters.Add("@email", SqlDbType.VarChar).Value = contact.email;
                    command.Parameters.Add("@ECFirstName", SqlDbType.VarChar).Value = string.IsNullOrEmpty(contact.ECFirstName) ? "NA" : contact.ECFirstName;
                    command.Parameters.Add("@ECLastName", SqlDbType.VarChar).Value = string.IsNullOrEmpty(contact.ECLastName) ? "NA" : contact.ECLastName;
                    command.Parameters.Add("@ECRelationship", SqlDbType.VarChar).Value = string.IsNullOrEmpty(contact.ECRelationship) ? "NA" : contact.ECRelationship;
                    command.Parameters.Add("@ECPhone", SqlDbType.VarChar).Value = string.IsNullOrEmpty(contact.ECPhone) ? "NA" : contact.ECPhone;

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    TempData["SuccessMessage"] = "You have successfully created contact info for the patient!";
                }
            }
            ModelState.Clear();
            return View();
        }
    }
}
