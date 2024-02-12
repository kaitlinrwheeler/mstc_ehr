using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using EHRApplication.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EHRApplication.Controllers
{
    // Controller responsible for handling user login and registration
    public class Login_Register : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly ILogger<Login_Register> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        public Account Input { get; set; }

        // Constructor to initialize required services
        public Login_Register(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<Login_Register> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _userStore = userStore;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;

            //Retrieving a list of the Roles from the database
            Input = new Account()
            {
                RoleList = _roleManager.Roles.Select(x => x.Name).Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i
                })
            };
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
        public async Task<IActionResult> Login(Account account)
        {
            if (ModelState.IsValid)
            {
                // Attempt to sign in the user with the provided email and password
                var result = await _signInManager.PasswordSignInAsync(account.Email, account.Password, true, lockoutOnFailure: false);

                // Check if the sign-in attempt was successful
                if (result.Succeeded)
                {
                    // Log information about the successful login
                    _logger.LogInformation("User logged in.");

                    // Redirect the user to the returnUrl if it's provided
                    return RedirectToAction("Index");
                }

                // Check if two-factor authentication is required
                if (result.RequiresTwoFactor)
                {
                    // If two-factor authentication is required, redirect to the login page with 2FA
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }

                // Check if the user's account is locked out
                if (result.IsLockedOut)
                {
                    // If the user's account is locked out, log a warning and redirect to the lockout page
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    // If none of the above conditions are met, it's an invalid login attempt
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If the ModelState is not valid, redisplay the login form
            return Page();
        }


        // Action method to display the Register view
        public IActionResult Register()
        {
            return View();
        }

        // Action method to handle the registration form submission
        [HttpPost]
        public async Task<IActionResult> Register(Account account)
        {
            ///returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                //Creates the instance of the user
                var user = CreateUser();

                // Set user name and create the user
                await _userStore.SetUserNameAsync(user, account.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, account.Password);

                if (result.Succeeded)
                {
                    // Log information about the successful creation of a new account
                    _logger.LogInformation("User created a new account with password.");

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
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        // If email confirmation is not required, sign in the user
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        // Redirect to the login page
                        return RedirectToAction("Login");
                    }
                }

                // Add model errors if user creation fails
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return View();
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
    }
}
