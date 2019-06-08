using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using CricketApp.Data;
using CricketApp.Domain;
using IdentityDemo.Models.AccountViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RazorHtmlToPdfDemo.Services;
using WebApp.Extentions;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Controllers
{

    [Route("[controller]/[action]")]
    public class AccountController : BaseController
    {
        private readonly CricketContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IRazorViewToStringRenderer razorViewToStringRenderer;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            CricketContext cricketContext,
            // IEmailSender emailSender,
            ILogger<AccountController> logger,
            IRazorViewToStringRenderer razorViewToStringRenderer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            // _emailSender = emailSender;
            _logger = logger;
            this.razorViewToStringRenderer = razorViewToStringRenderer;
            _context = cricketContext;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ViewBag.Name = "Login";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult UserProfile(string userName)
        {
            if (User.Identity.IsAuthenticated)
            {
                //var userProfile = await _userManager.Users
                //    .SingleOrDefault(m => m.UserId == userId);
                //return (userProfile);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.userName, model.Password, model.isPersistent, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    if (returnUrl != null)
                       {
                        return LocalRedirect(returnUrl);
                      }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }
                else
                {
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToAction(nameof(LoginWith2fa), new { returnUrl, model.isPersistent });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
                //return RedirectToAction("Index", "Home");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWith2fa(bool rememberMe, string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var model = new LoginWith2faViewModel { RememberMe = rememberMe };
            ViewData["ReturnUrl"] = returnUrl;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWith2fa(LoginWith2faViewModel model, bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, model.RememberMachine);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID {UserId} logged in with 2fa.", user.Id);
                return RedirectToLocal(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                _logger.LogWarning("Invalid authenticator code entered for user with ID {UserId}.", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);

            var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID {UserId} logged in with a recovery code.", user.Id);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                _logger.LogWarning("Invalid recovery code entered for user with ID {UserId}", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewBag.Name = "Register";
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            var conn = _context.Database.GetDbConnection();
            _logger.LogInformation($"Database: {conn.Database}");
            _logger.LogInformation($"Server: {conn.DataSource}");
            //    var Roles = _context.UserRole.ToList();
            ViewBag.Teams = new SelectList(_context.Teams, "TeamId", "Team_Name");
            ViewBag.RoleName = new SelectList(_context.Roles
               .Where(i => i.Id != 19)
                , "NormalizedName");
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                
                if (result.Succeeded)
                {

                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var url = Request.Host.Value;

                    var link = Url.Action(
                      "ConfirmEmail",
                      "Account",
                      values: new { userId = user.Id, code = code },
                      protocol: Request.Scheme
                      );

                    //var link = $"https://{url}/Account/ConfirmEmail?userId={user.Id}&code={code}";
                    var htmlString = await razorViewToStringRenderer.RenderViewToStringAsync("EmailTemplate", new EmailTemplate
                    {
                        Title = "Email Confirmation",
                        Body = $"<a target=\"_blank\" href=\"{HtmlEncoder.Default.Encode(link)}\">Yes it belongs to me</a>",
                        Message = " <p>Assalam-o-Alaikum</p> <p>Hello Admin</p> <p>Please confirm, if this user " + model.UserName + " with the email " + model.Email + " belongs to you</P>"

                    });
                    string adminEmail = "takecarebudy@gmail.com";
                    string subject = "Email Confirmation";
                    //if (model.RoleName == "Club User")
                    //{
                    //    adminEmail = _context.ClubAdmins
                    //     .AsNoTracking()
                    //     .Where(i => i.TeamId == model.TeamId)
                    //     .Select(i => i.User.Email)
                    //     .Single();
                    //}
                    //else
                    //{

                    //    adminEmail = "farazahmed879@yahoo.com";
                    //}


                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("User created a new account with password.");

                    // await _userManager.AddToRoleAsync(user, model.RoleName);

                    await EmailExtensions.Execute(adminEmail, model.UserName, htmlString, subject);

                    return RedirectToAction("PendingRequest", "Account");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Index", "Home");
        }



        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {

            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            // var id = Int32.Parse(userId);

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {

                var url = Request.Host.Value;


                var link = Url.Action(
                  "Login",
                  "Account",
                  values: new { email = user.Email },
                  protocol: Request.Scheme
                  );

                //var link = $"https://{url}/Account/ConfirmEmail?userId={user.Id}&code={code}";
                var htmlString = await razorViewToStringRenderer.RenderViewToStringAsync("EmailTemplate", new EmailTemplate
                {
                    Title = "Congratulation",
                    Body = $"<a target=\"_blank\" href=\"{HtmlEncoder.Default.Encode(link)}\">ScoreExec</a>",
                    Message = " <p>Assalam-o-Alaikum</p> <p>Dear User</p> <p>You are approved by the admin</p> <p>User Name: </P>" + user.UserName + "<p>Email: </p>" + user.Email
                });

                string subject = "Email Confirmation";
                await _signInManager.SignInAsync(user, isPersistent: false);
                await EmailExtensions.Execute(user.Email, user.UserName, htmlString, subject);
            }

            return RedirectToAction("ApprovedUser", "Account");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ApprovedUser()
        {

            return View();
        }

        [HttpGet]
        public IActionResult PendingRequest()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction(nameof(ForgotPasswordConfirmation));
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                var url = Request.Host.Value;

                string adminEmail = model.Email;
                string subject = "Reset Password";

                var link = $"https://{url}/Account/ResetPassword?userId={user.Id}&code={code}";
                var htmlString = await razorViewToStringRenderer.RenderViewToStringAsync("EmailTemplate", new EmailTemplate
                {
                    Title = "Reset Password",
                    Body = $"<a target=\"_blank\" href=\"{link}\">Click here</a>",
                    Message = "<p>Asslam-o-Alaikum</p> <p>Dear User</p> <p>Click the below link to change or reset your passowrd</p>"

                });

                await EmailExtensions.Execute(adminEmail, null, htmlString, subject);
                //var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
                //await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                //$"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
                return RedirectToAction(nameof(PendingResertPasswordRequest));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult PendingResertPasswordRequest()
        {

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {

            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }
            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            AddErrors(result);
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {

            return View();
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }



        [HttpGet]
        public IActionResult IsEmailAvailable(string email)
        {
            return Json(_context.Users
                .AsNoTracking()
                .Any(i => i.Email == email));
        }

        [HttpGet]
        public IActionResult IsUserAvailable(string user)
        {
            return Json(_context.Users
                .AsNoTracking()
                .Any(i => i.UserName == user));
        }




        #endregion
    }
}