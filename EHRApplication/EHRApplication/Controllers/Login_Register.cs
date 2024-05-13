using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using EHRApplication.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using EHRApplication.Services;
using Newtonsoft.Json.Linq;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Mailjet.Client.TransactionalEmails;
using Microsoft.Identity.Client;
using NuGet.ContentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace EHRApplication.Controllers
{
    // Controller responsible for handling user login and registration
    public class Login_Register : BaseController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly ILogger<Login_Register> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IListService _listService;
        private readonly ILogService _logService;
        //This is the api codes in order to route to the right smtp server to sent an email.
        MailJetKeys keys = new MailJetKeys();

        public LoginAccount Input { get; set; }

        // Constructor to initialize required services
        public Login_Register(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<Login_Register> logger,
            IEmailSender emailSender, 
            ILogService logService, 
            IConfiguration configuration,
            IListService listService,
            RoleManager<IdentityRole> roleManager)
            :base(logService, listService, configuration)
        {
            _userManager = userManager;
            _userStore = userStore;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _listService = listService;
            _logService = logService;
    }

        // Action method to display the index view
        public IActionResult Index()
        {
            return View();
        }

        // Action method to display the Login view
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginAccount account)
        {
            //Removing the model state for the password and email.
            ModelState.Remove("Password");
            ModelState.Remove("Email");
            //Testing for nulls and if so will return with nulls
            if (ModelState.IsValid)
            {
                // Attempt to sign in the user with the provided email and password
                var result = await _signInManager.PasswordSignInAsync(account.Email, account.Password, true, lockoutOnFailure: false);

                // Check if the sign-in attempt was successful
                if (result.Succeeded)
                {
                    // Log information about the successful login
                    _logger.LogInformation("User logged in.");
                    TempData["SuccessMessage"] = "You have successfully logged in!";


                    // Redirect the user to the returnUrl if it's provided
                    return RedirectToAction("UserDashboard", "Home");
                }
                else
                {
                    ModelState.AddModelError("Password", "Invalid Email or Password.");
                }

                // Check if two-factor authentication is required
                if (result.RequiresTwoFactor)
                {
                    // If two-factor authentication is required, redirect to the login page with 2FA
                    //return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }

                // Check if the user's account is locked out
                if (result.IsLockedOut)
                {
                    // If the user's account is locked out, log a warning and redirect to the lockout page
                    _logger.LogWarning("User account locked out.");
                    //return RedirectToPage("./Lockout");
                }
                else
                {
                    // If none of the above conditions are met, it's an invalid login attempt
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    //       return Page();
                    return View();
                }
            }
            else
            {
                //Will set the errors for the input if they are null
                if(account.Email == null)
                {
                ModelState.AddModelError("Email", "Email must not be null");
                }
                if(account.Password == null)
                {
                ModelState.AddModelError("Password", "Password must not be null");
                }
            }

            // If the ModelState is not valid, redisplay the login form
            return View(account);
        }


        // Action method to display the Register view
        public IActionResult Register()
        {
            return View();
        }

        // Action method to handle the registration form submission
        [HttpPost]
        public async Task<IActionResult> Register(RegisterAccount account)
        {
            //Testing for nulls and if so will return with nulls.
            if (ModelState.IsValid)
            {
                //Testing to make sure that the email has "@mstc.edu" in it.
                if(!System.Text.RegularExpressions.Regex.IsMatch(account.Email, @"^[^\s]+@mstc\.edu$")) { 
                    //sets the error message for the email
                    ModelState.AddModelError("Email", "Email must end with @mstc.edu."); 
                    return View(account); }

                //Testing to make sure that the password is greater than 8
                if (account.Password.Length >= 8)
                {
                    //Tests and makes sure that the password has at least one upper, lower, number and special character
                    if(!account.Password.Any(char.IsUpper) || !account.Password.Any(char.IsLower) || !account.Password.Any(char.IsDigit) || !account.Password.Any(IsSpecialCharacter)) {
                        //sets the error message for password
                        ModelState.AddModelError("Password", "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character."); 
                        return View(account);
                    }
                    if(account.ConfirmPassword != account.Password)
                    {
                        //sets the error message for confirmpassword
                        ModelState.AddModelError("ConfirmPassword", "The password and confirmation password do not match."); 
                        return View(account);
                    }
                }
                else
                {
                    //sets the error message for password
                    ModelState.AddModelError("Password", "Password must have more than 8 character"); 
                    return View(account);
                }


                //Creates the instance of the user
                var user = CreateUser();

                // Set username, fist and last name
                await _userStore.SetUserNameAsync(user, account.Email, CancellationToken.None);
                user.Firstname = account.Firstname;
                user.Lastname = account.Lastname;
                user.Email = account.Email;

                //Links user instance to the user info
                var result = await _userManager.CreateAsync(user, account.Password);

                if (result.Succeeded)
                {
                    // Log information about the successful creation of a new account
                    _logger.LogInformation("User created a new account with password.");
                    //This will set the role for the user being created. (setting as student because thats how we make sure they r not admin)
                    await _userManager.AddToRoleAsync(user, "Student");

                    // Retrieve the user's ID
                    var userId = await _userManager.GetUserIdAsync(user);

                    // Generate an email confirmation token for the user
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    // Encode the confirmation token to be sent via URL
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    // Check if email confirmation is required
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        // If email confirmation is required, redirect to the index page
                        return RedirectToAction("UserDashboard", "Home");
                    }
                    else
                    {
                        // If email confirmation is not required, sign in the user
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        TempData["RegisterMessage"] = "You have successfully created your account!";

                        // Redirect to the login page
                        return RedirectToAction("Login");
                    }
                }

                // Add model errors if user creation fails
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                //sets a message that will be displayed when returned to the view
                TempData["ErrorMessage"] = "An account with that email is already been created. Please make sure you entered your email in correctly";
                return View(account);
            }
            else
            {
                //Will set the errors for the input if they are null
                if (account.Email == null)
                {
                    ModelState.AddModelError("Email", "Email must not be empyt");
                }
                if (account.Password == null)
                {
                    ModelState.AddModelError("Password", "Password must not be empty");
                }
                if(account.ConfirmPassword == null)
                {
                    ModelState.AddModelError("ConfirmPassword", "ConfimrPassword must not be empty.");
                }
            }
            // If we got this far, something failed, redisplay form
            return View(account);
        }

        
        public async Task<IActionResult> Logout()
        {
            //This will close the previously open user.
            await _signInManager.SignOutAsync();
            //log that the user was logged add perameters based on what is needeed to log something.
            _logger.LogInformation("User logged out.");
            TempData["LogoutMessage"] = "You have successfully logged out!";

            //Redirect to a view. sending the user to the login page as thought that was acceptable
            return RedirectToAction("Login");
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                //Creates an instance of the user that is registering
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private bool IsSpecialCharacter(char c)
        {
            // Define a set of special characters
            string specialCharacters = @"!@#$%^&*()-_=+[]{}|;:',.<>?";

            // Check if the character is in the set of special characters
            return specialCharacters.Contains(c);
        }

        /// <summary>
        /// This page will show the page to enter an email to send a code.
        /// </summary>
        /// <returns></returns>
        public IActionResult ForgotPassword()
        {            
            return View();
        }

        /// <summary>
        /// This will check and make sure that there is an email linked to the one entered and send a verification code to the email.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(RegisterAccount account)
        {
            //Making sure the user entered in an email.
            if (!account.Email.IsNullOrEmpty())
            {
                //Testing to make sure that the email has "@mstc.edu" in it.
                if (!System.Text.RegularExpressions.Regex.IsMatch(account.Email, @"^[^\s]+@mstc\.edu$"))
                {
                    //sets the error message for the email
                    ModelState.AddModelError("Email", "Email must end with @mstc.edu.");
                    return View(account);
                }

                //This will try to get a user using the email entered.
                var user = await _userManager.FindByEmailAsync(account.Email);

                //This will make sure the email entered is connected to a user.
                if (user == null)
                {
                    //Creating an error for the email as it was not linked to a user.
                    ModelState.AddModelError("Email", "No account linked to that email.");
                    return View();
                }

                //This will be the verification code sent to the email.
                Random ranVariable = new Random();
                int verificationCode = ranVariable.Next(10000, 99999);
                var verificationToken = await _userManager.GeneratePasswordResetTokenAsync(user);

                //Creating a mail client connection to the SMTP server.
                MailjetClient client = new MailjetClient(keys.apiKeyPublic, keys.apiKeyPrivate);

                //Creating a new request to send the email.
                MailjetRequest request = new MailjetRequest
                {
                    Resource = Send.Resource,
                };

                //This is the email itself
                var email = new TransactionalEmailBuilder()
                    .WithFrom(new SendContact("MyEHR12@gmail.com"))
                    .WithSubject("Verification Code")
                    .WithHtmlPart("<h1>Your verification code is: "+ verificationCode +". This code will expire in 10 minutes.</h1>")
                    .WithTo(new SendContact(account.Email))
                    .Build();

                //Invoke API and send email
                var response = await client.SendTransactionalEmailAsync(email);

                //Checking the response
                Assert.AreEqual(1, response.Messages.Length);


                //Sending the verification code to the database to be grabbed when the user tries to login.
                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    //This will go and delete all the verification codes from the table that are expired.
                    string sql = "DELETE FROM Verifications WHERE DateTime < DATEADD(minute, -10, GETDATE());";
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();

                    //SQL query that is going to insert the data that the verification code into the table.
                    sql = "INSERT INTO [Verifications] (VerificationCode, VerificationToken) " +
                        "VALUES (@VerificationCode, @VerificationToken)";

                    using (cmd = new SqlCommand(sql, connection))
                    {
                        cmd.CommandType = CommandType.Text;

                        //adding parameters
                        cmd.Parameters.Add("@VerificationCode", SqlDbType.Int).Value = verificationCode;
                        cmd.Parameters.Add("@VerificationToken", SqlDbType.VarChar).Value = verificationToken;

                        connection.Open();
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            else { return View(account); }

            //Going to the page that will enter the verification code in.
            return RedirectToAction("CheckVerification", new {email = account.Email});
        }

        /// <summary>
        /// This is the page where you will enter in a new password.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult PasswordVerification(string email, int code)
        {
            //This will take the user back to the page to send the verification code because they didn't get here with they way that they should have.
            if(email.IsNullOrEmpty() || code == 0)
            {
                return RedirectToAction("ForgotPassword");
            }

            //Setting the objects email field to the email that the user has entered into the previous page.
            Verification account = new Verification();
            account.Email = email;
            account.VerificationCode = code;

            //Displays the verificationPage
            return View(account);
        }
        
        /// <summary>
        /// This will chagne the password.
        /// </summary>
        /// <param name="verifiedInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PasswordVerification(Verification verifiedInfo)
        {
            //This will try and find the user in the database using the email.
            var user = await _userManager.FindByEmailAsync(verifiedInfo.Email);
            if(user == null)
            {
                //Don't update the password for anyones because user does not exist.
                return RedirectToAction("ForgotPassword");
            }

            //This tests to make sure that the password and confirm password are not null.
            if (verifiedInfo.Password.IsNullOrEmpty())
            {
                ModelState.AddModelError("Password", "Password is required.");
                if (verifiedInfo.ConfirmPassword.IsNullOrEmpty())
                {
                    ModelState.AddModelError("Confirmpassword", "Confirmation password is required.");
                }
                return View(verifiedInfo);
            }

            //Testing to make sure that the password is greater than 8
            if (verifiedInfo.Password.Length >= 8)
            {
                //Tests and makes sure that the password has at least one upper, lower, number and special character
                if (!verifiedInfo.Password.Any(char.IsUpper) || !verifiedInfo.Password.Any(char.IsLower) || !verifiedInfo.Password.Any(char.IsDigit) || !verifiedInfo.Password.Any(IsSpecialCharacter))
                {
                    //sets the error message for password
                    ModelState.AddModelError("Password", "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.");
                    return View(verifiedInfo);
                }
                //Tests and makes sure the password and confirmpassword are the same.
                if (verifiedInfo.ConfirmPassword != verifiedInfo.Password)
                {
                    //sets the error message for confirmpassword
                    ModelState.AddModelError("ConfirmPassword", "The password and confirmation password do not match.");
                    return View(verifiedInfo);
                }
            }
            else
            {
                //sets the error message for password
                ModelState.AddModelError("Password", "Password must have more than 8 character");
                return View(verifiedInfo);
            }

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                //This will select the token that will be used to change the users password.
                string sql = "SELECT VerificationToken FROM Verifications WHERE VerificationCode = @Code";
                SqlCommand cmd = new SqlCommand(sql, connection);

                // Replace placeholder with parameter to avoid SQL injection.
                cmd.Parameters.AddWithValue("@Code", verifiedInfo.VerificationCode);
                connection.Open();
                string verifiedToken = cmd.ExecuteScalar() as string;
                verifiedInfo.VerificationToken = verifiedToken;
                connection.Close();

                //Making sure that I have grabbed the token before deleting the record from the database.
                if (!verifiedInfo.VerificationToken.IsNullOrEmpty())
                {
                    // Sql query.
                    sql = "DELETE FROM [dbo].[Verifications] WHERE VerificationCode = VerificationCode";

                    cmd = new SqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@VerificationCode", verifiedInfo.VerificationCode);
                    connection.Open();
                    // Execute the SQL command.
                    int rowsAffected = cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }

            //This will try to reset the password for the user.
            var result = await _userManager.ResetPasswordAsync(user, verifiedInfo.VerificationToken, verifiedInfo.Password);
            if(result.Succeeded)
            {
                return RedirectToAction("Login");
            }

            return RedirectToAction("Forgotpassword");
        }

        /// <summary>
        /// This view will allow for the code to be entered.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public IActionResult CheckVerification(string email)
        {
            //Setting the objects email field to the email that the user has entered into the previous page.
            Verification account = new Verification();
            account.Email = email;

            //Displays the verificationPage
            return View(account);
        }

        /// <summary>
        /// This will check the code and make sure it is in the database and valid.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CheckVerification(Verification account)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                //Creating variables.
                List<Verification> verifiedInfoList = new List<Verification>();
                bool goodRequest = false;
                DateTime currentTime = DateTime.Now;

                //Selecting the verification code from the  database.
                string sql = "SELECT VerificationCode, DateTime FROM Verifications";
                SqlCommand cmd = new SqlCommand(sql, connection);
                connection.Open();
                //Grabbing the data from the table
                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Verification verification = new Verification();

                        verification.VerificationCode = Convert.ToInt32(dataReader["VerificationCode"]);
                        verification.DateTime = Convert.ToDateTime(dataReader["DateTime"]);
                        verifiedInfoList.Add(verification);
                    }
                }
                connection.Close();

                //Running through all of the verification codes in the database and seeing if any of them match the code that the user entered.
                foreach (Verification v in verifiedInfoList)
                {
                    //If true will move on and allow the user to enter in a new password.
                    if(account.VerificationCode == v.VerificationCode)
                    {
                        return RedirectToAction("PasswordVerification", new {email = account.Email, code = account.VerificationCode});
                    }
                }
                //Will show error on the page.
                ModelState.AddModelError("VerificationCode", "That code is invalid, has expired or has already been used.");
                return View(account);
            }
        }
    }
}
