using Core.Database;
using Core.Models;
using Core.ViewModels;
using Logic.Helpers;
using Logic.IHelpers;
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

    }
}
