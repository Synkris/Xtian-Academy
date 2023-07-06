using Core.Database;
using Core.Models;
using Core.ViewModels;
using Logic.Helpers;
using Logic.IHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace Xtian_Academy.Controllers
{
    public class AccountsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IDropdownHelper _dropdownHelper;
        private readonly IApplicationHelper _applicationHelper;
        private readonly IUserHelper _userHelper;
        private readonly IEmailHelper _emailHelper;

        public AccountsController(AppDbContext context, UserManager<ApplicationUser> userManger, SignInManager<ApplicationUser> signInManager, IDropdownHelper dropdownHelper, IUserHelper userHelper, IApplicationHelper applicationHelper, IEmailHelper emailHelper)
        {
            _context = context;
            _userManager = userManger;
            _signInManager = signInManager;
            _dropdownHelper = dropdownHelper;
            _applicationHelper = applicationHelper;
            _userHelper = userHelper;
            _emailHelper = emailHelper;
        }

        public IActionResult Index()
        {
            return View();
        }

        //APPLICATION GET ACTION
        [HttpGet]
        public IActionResult Application()
        {
            ViewBag.YesOrNo = _dropdownHelper.GetDropDownEnumsList();
            return View();
        }



        //REGISTRAION POST 
        [HttpPost]
        public async Task<JsonResult> Application(string applicationUserViewModel)
        {
            try
            {
                var newApplicant = JsonConvert.DeserializeObject<ApplicationUserViewModel>(applicationUserViewModel);

                //We did all the validations
                ViewBag.YesOrNo = _dropdownHelper.GetDropDownEnumsList();

                // Query the user details if it exists in the Db B4 Authentication
                var emailCheck = await _userHelper.FindByEmailAsync(newApplicant.Email);
                var usernameCheck = await _userHelper.FindByUserNameAsync(newApplicant.UserName);
                var phoneCheck = await _userHelper.FindByPhoneNumberAsync(newApplicant.PhoneNumber);
                if (emailCheck != null)
                {
                    return Json(new { isError = true, msg = "Email already exist" });
                }
                if (usernameCheck != null)
                {
                    return Json(new { isError = true, msg = "UserName already exist" });
                }
                if (phoneCheck != null)
                {
                    return Json(new { isError = true, msg = "Phone number already exist" });
                }
                // End of Query  4  the user details if it exists in the Db B4 Authentication


                var returndResultFrmRegisterService = _applicationHelper.RegisterApplicantService(newApplicant).Result;
                if (returndResultFrmRegisterService != null)
                {
                    var userToken = await _emailHelper.CreateUserToken(newApplicant.Email);
                    var addToRole = _userManager.AddToRoleAsync(returndResultFrmRegisterService, "Applicant").Result;
                    if (addToRole.Succeeded & userToken != null)
                    {
                        string linkToClick = HttpContext.Request.Scheme.ToString() + "://"
                        + HttpContext.Request.Host.ToString() + "/Accounts/EmailVerified?token=" + userToken.Token;
                        _emailHelper.VerificationEmail(newApplicant.Email, linkToClick);
                        return Json(new { isError = false, msg = "Registeration successful, Check your mail to complete application" });

                    }
                }
                return Json(new { isError = true, msg = "Application Failed" });
            }
            catch (Exception ex)
            {
                return Json(new { isError = true, msg = "An unexpected error occured " + ex.Message });
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Login POST ACTION
        [HttpPost]
        public async Task<JsonResult> Login(string loginViewModel)
        {
            var userDetails = JsonConvert.DeserializeObject<LoginViewModel>(loginViewModel);
            var user = await _userHelper.FindByEmailAsync(userDetails.Email);
            if (user != null)
            {
                if (user.EmailConfirmed && !user.Deactivated)
                {
                    var currentUser = _applicationHelper.AuthenticateUser(userDetails).Result;
                    if (currentUser != null)
                    {
                        var dashboard = _applicationHelper.GetUserDashboardPage(user);
                        return Json(new { isError = false, msg = "Welcome!", dashboard = dashboard });
                    }
                }
                else
                {
                    if (!user.EmailConfirmed)
                    {
                        var url = "/Accounts/UnverifiedAccount";
                        return Json(new { isNotVerified = true, msg = "Email Unverifed!!!", url = url });
                    }
                    if (user.Deactivated)
                    {
                        var url = "/Accounts/Login";
                        return Json(new { isNotVerified = true, msg = "Account Deactivated!!! Please contact Admin", url = url });
                    }
                }
            }
            return Json(new { isError = true, msg = "Login Failed" });
        }
        public IActionResult LogOut()
        {
            _applicationHelper.LogOut();
            return RedirectToAction("Index", "Home");
        }

		// ChangePassword GET ACTION
		[Authorize]
		public IActionResult ChangePassword()
		{
			ViewBag.Layout = _applicationHelper.GetUserLayout(User.Identity.Name);
			return View();
		}

		// ChangePassword POST ACTION
		[HttpPost]
		public async Task<JsonResult> ChangePasswordPost(string userPasswordDetails)
		{
			if (userPasswordDetails != null)
			{
				var passDetails = JsonConvert.DeserializeObject<ApplicationUserViewModel>(userPasswordDetails);
				if (passDetails.NewPassword != passDetails.ConfirmPassword)
				{
					return Json(new { isError = true, msg = "Password and Confirm Pasword not the same" });
				}
				var currentUser = _userHelper.FindByUserNameAsync(User.Identity.Name).Result;
				var result = await _userManager.ChangePasswordAsync(currentUser, passDetails.OldPassword, passDetails.NewPassword);
				if (result.Succeeded)
				{
					string loginLink = HttpContext.Request.Scheme.ToString() + "://"
						+ HttpContext.Request.Host.ToString() + "/Accounts/Login";

					_emailHelper.ChangePasswordAlert(currentUser, loginLink);
					var linkToClick = "/Accounts/ChangePassword";
					return Json(new { isError = false, msg = "Password Changed Successfully", url = linkToClick });
				}
				return Json(new { isError = true, msg = "Failed" });
			}
			return Json(new { isError = true, msg = "Input the required fields" });
		}

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> ForgotPassword(ApplicationUser applicationUser)
        {
            try
            {
                if (applicationUser != null)
                {
                    var user = await _userHelper.FindByEmailAsync(applicationUser.Email);
                    if (user != null)
                    {
                        var userToken = await _userHelper.CreateUserToken(applicationUser.Email);
                        if (userToken != null)
                        {
                            await _context.SaveChangesAsync();
                            ModelState.Clear();

                            string linkToClick = HttpContext.Request.Scheme.ToString() + "://" +
                               HttpContext.Request.Host.ToString() + "/Accounts/ResetPassword?token=" + userToken.Token;

                            _emailHelper.ForgotPasswordTemplateEmailer(userToken.User, linkToClick);

                            return Json(new { isNotVerified = false, msg = ("A password reset link has been sent to your email address please check your email for the next action.") });
                        }
                        else
                        {
                            return Json(new { isNotVerified = true, msg = ("A password reset failed.") });

                        }
                    }
                    else
                    {
                        return Json(new { isNotVerified = true, msg = ("Account Not Found") });

                    }
                }
                else
                {
                    return Json(new { isNotVerified = true, msg = ("A password reset failed.") });

                }
            }
            catch (Exception ex)
            {
                return Json(new { isError = true, msg = "An unexpected error occured " + ex.Message });

            }
        }

        public IActionResult EmailVerified(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                Guid userToken = Guid.Parse(token);

                var userVerification = _userHelper.GetUserToken(userToken).Result;
                if (userVerification == null || userVerification.Token == Guid.Empty)
                {
                    return RedirectToAction("Login");
                }
                if (userVerification.User.EmailConfirmed)
                {
                    return View();
                }
                if (userVerification.Used)
                {
                    return View();
                }
                else
                {
                    userVerification.Used = true;
                    userVerification.DateUsed = DateTime.Now;
                    userVerification.User.EmailConfirmed = true;

                    _context.Update(userVerification);
                    _context.Update(userVerification.User);

                    var sendemail = _emailHelper.Gratitude(userVerification.User.Email);
                    _context.SaveChanges();
                    return View();
                }
            }
            return RedirectToAction("Login");
        }

        public IActionResult UnverifiedAccount()
        {
            return View();
        }

        // ADMIN REGISTRAION GET ACTION
        [HttpGet]
        public IActionResult RegisterAdmin()
        {
            return View();
        }

        // ADMIN REGISTRAION POST 
        [HttpPost]
        public async Task<JsonResult> AdminRegisteration(string applicationUserViewModel)
        {
            try
            {
                var newApplicant = JsonConvert.DeserializeObject<ApplicationUserViewModel>(applicationUserViewModel);

                // Query the user details if it exists in the Db B4 Authentication
                var emailCheck = await _userHelper.FindByEmailAsync(newApplicant.Email);
                if (emailCheck != null)
                {
                    return Json(new { isError = true, msg = "Email already exist" });
                }
                // End of Query  4  the user details if it exists in the Db B4 Authentication

                var returndResultFrmRegisterService = _applicationHelper.RegisterAdminService(newApplicant).Result;
                if (returndResultFrmRegisterService != null)
                {
                    var userToken = await _emailHelper.CreateUserToken(newApplicant.Email);
                    var addToRole = _userManager.AddToRoleAsync(returndResultFrmRegisterService, "Admin").Result;
                    if (addToRole.Succeeded & userToken != null)
                    {
                        string linkToClick = HttpContext.Request.Scheme.ToString() + "://"
                        + HttpContext.Request.Host.ToString() + "/Accounts/EmailVerified?token=" + userToken.Token;
                        _emailHelper.VerificationEmail(newApplicant.Email, linkToClick);
                        return Json(new { isError = false, msg = "Registeration successful, Check your mail to complete application" });

                    }
                }
                return Json(new { isError = true, msg = "Application Failed" });
            }
            catch (Exception ex)
            {
                return Json(new { isError = true, msg = "An unexpected error occured " + ex.Message });
            }
        }

    }
}
