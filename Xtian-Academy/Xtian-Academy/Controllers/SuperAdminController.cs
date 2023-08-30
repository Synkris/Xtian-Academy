using Core.Models;
using Logic.IHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Xtian_Academy.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class SuperAdminController : Controller
    {
        private readonly IApplicationHelper _applicationHelper;
        private readonly ISuperAdminHelper _superAdminHelper;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private const string Activate_ActionType = "Activate";
        private const string Deactivate_ActionType = "Deactivate";

        public SuperAdminController(IApplicationHelper applicationHelper, ISuperAdminHelper superAdminHelper, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _applicationHelper = applicationHelper;
            _superAdminHelper = superAdminHelper;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        public IActionResult ManageAdmin()
        {
            ViewBag.Layout = _applicationHelper.GetUserLayout(User.Identity.Name);
            var allAdmin = _userManager.GetUsersInRoleAsync("Admin").Result;
            return View(allAdmin);
        }

        // POST ACTION FOR ADMIN ACTIVATION AND DEACTIVATION
        [HttpPost]
        public JsonResult AdminTurnOnAndOff(string collectedAdminData)
        {
            try
            {
                if (collectedAdminData != null)
                {
                    var adminAccount = JsonConvert.DeserializeObject<ApplicationUser>(collectedAdminData);
                    if (adminAccount.ActionType == Activate_ActionType)
                    {
                        if (adminAccount.Id != null)
                        {
                            var adminToActivate = _superAdminHelper.ActivateAdminAccount(adminAccount);
                            if (adminToActivate != null)
                            {
                                return Json(new { isError = false, msg = "Activated Successfully" });
                            }
                        }

                    }
                    else if (adminAccount.ActionType == Deactivate_ActionType)
                    {
                        if (adminAccount.Id != null)
                        {
                            var adminToDectivate = _superAdminHelper.DeactivateAdminAccount(adminAccount);
                            if (adminToDectivate != null)
                            {
                                return Json(new { isError = false, msg = "Deactivated Successfully" });
                            }
                        }
                    }
                }
                return Json(new { isError = true, msg = "Failed" });
            }
            catch (Exception ex)
            {
                return Json(new { isError = true, msg = "An unexpected error occured " + ex.Message });
            }
        }
    }
}
