using EHRApplication.Models;
using EHRApplication.Services;
using EHRApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EHRApplication.Controllers
{
    public class ContactController : BaseController
    {
        private readonly ILogService _logService;
        private readonly string _connectionString;
        private readonly IListService _listService;

        public ContactController(ILogService logService, IListService listService, IConfiguration configuration)
            : base(logService, listService, configuration)
        {
            _logService = logService;
            this._connectionString = base._connectionString;
            _listService = listService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(PatientContact contact)
        {
            if (contact.MHN == 0)
            {
                ModelState.AddModelError("MHN", "Please select a patient.");
            }
            //returns the model if null because there were errors in validating it
            if (!ModelState.IsValid)
            {
                return View(contact);
            }

            using (SqlConnection connection = new SqlConnection(this._connectionString))
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
                    command.Parameters.Add("@email", SqlDbType.VarChar).Value = string.IsNullOrEmpty(contact.email) ? "" : contact.email;
                    command.Parameters.Add("@ECFirstName", SqlDbType.VarChar).Value = string.IsNullOrEmpty(contact.ECFirstName) ? "" : contact.ECFirstName;
                    command.Parameters.Add("@ECLastName", SqlDbType.VarChar).Value = string.IsNullOrEmpty(contact.ECLastName) ? "" : contact.ECLastName;
                    command.Parameters.Add("@ECRelationship", SqlDbType.VarChar).Value = string.IsNullOrEmpty(contact.ECRelationship) ? "" : contact.ECRelationship;
                    command.Parameters.Add("@ECPhone", SqlDbType.VarChar).Value = string.IsNullOrEmpty(contact.ECPhone) ? "" : contact.ECPhone;

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    TempData["SuccessMessage"] = "You have successfully created contact info for the patient!";
                }
            }
            ModelState.Clear();
            return View();
        }

        [HttpGet]
        public IActionResult EditContact(int mhn)
        {
            PortalViewModel portalViewModel = new PortalViewModel();
            portalViewModel.PatientDemographic = _listService.GetPatientByMHN(mhn);

            // Gets patient contact object
            portalViewModel.PatientContact = _listService.GetContactByMHN(mhn);

            ViewBag.Patient = portalViewModel.PatientDemographic;
            ViewBag.MHN = mhn;

            return View(portalViewModel);
        }

        [HttpPost]
        public IActionResult EditContact(PatientContact contact)
        {
            //returns the model if null because there were errors in validating it
            if (!ModelState.IsValid)
            {
                PortalViewModel portalViewModel = new PortalViewModel();
                portalViewModel.PatientDemographic = _listService.GetPatientByMHN(contact.MHN);
                portalViewModel.PatientContact = contact;
                ViewBag.Patient = portalViewModel.PatientDemographic;
                ViewBag.MHN = contact.MHN;

                return View(portalViewModel);
            }

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                //SQL query that is going to insert the data that the user entered into the database table.
                string sql = "UPDATE [PatientContact] SET " +
                             "address = @address, " +
                             "city = @city, " +
                             "state = @state, " +
                             "zipcode = @zipcode, " +
                             "phone = @phone, " +
                             "email = @email, " +
                             "ECFirstName = @ECFirstName, " +
                             "ECLastName = @ECLastName, " +
                             "ECRelationship = @ECRelationship, " +
                             "ECPhone = @ECPhone " +
                             "WHERE patientContactId = @contactId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;


                    //The some of them test to see if the value if null or empty because they are optional on the form so if it is null or empty it will display NA otherwise will add the data enterd by the user.
                    //adding parameters
                    command.Parameters.Add("@contactId", SqlDbType.VarChar).Value = contact.patientContactId;
                    command.Parameters.Add("@address", SqlDbType.VarChar).Value = contact.address;
                    command.Parameters.Add("@city", SqlDbType.VarChar).Value = contact.city;
                    command.Parameters.Add("@state", SqlDbType.VarChar).Value = contact.state;
                    command.Parameters.Add("@zipcode", SqlDbType.VarChar).Value = contact.zipcode;
                    command.Parameters.Add("@phone", SqlDbType.VarChar).Value = contact.phone;
                    
                    
                    
                    command.Parameters.Add("@email", SqlDbType.VarChar).Value = string.IsNullOrEmpty(contact.email) ? "" : contact.email;


                    command.Parameters.Add("@ECFirstName", SqlDbType.VarChar).Value = string.IsNullOrEmpty(contact.ECFirstName) ? "" : contact.ECFirstName;
                    command.Parameters.Add("@ECLastName", SqlDbType.VarChar).Value = string.IsNullOrEmpty(contact.ECLastName) ? "" : contact.ECLastName;
                    command.Parameters.Add("@ECRelationship", SqlDbType.VarChar).Value = string.IsNullOrEmpty(contact.ECRelationship) ? "" : contact.ECRelationship;
                    command.Parameters.Add("@ECPhone", SqlDbType.VarChar).Value = string.IsNullOrEmpty(contact.ECPhone) ? "" : contact.ECPhone;

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            ModelState.Clear();
            return RedirectToAction("PatientOverview", "Patient", new { mhn = contact.MHN } );
        }

    }
}
