using Core.Database;
using Core.Enum;
using Core.Models;
using Core.ViewModels;
using Logic.IHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Xtian_Academy.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AdminController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IStudentHelper _studentHelper;
        private readonly IAdminHelper _adminHelper;
        private readonly IApplicationHelper _applicationHelper;
        private readonly IDropdownHelper _dropdownHelper;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly IEmailHelper _emailHelper;
        private const string Create_Training_Cost_ActionType = "CreateTrainingCourse";
        private const string Edit_Training_Cost_ActionType = "EditTrainingCourse";
        private const string Activate_Training_Cost_ActionType = "ActivateTrainingCourse";
        private const string Deactivate_Training_Cost_ActionType = "DeactivateTrainingCourse";
        private const string Delete_Training_Cost_ActionType = "DeleteTrainingCourse";

        public AdminController(IUserHelper userHelper, IApplicationHelper applicationHelper, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IAdminHelper adminHelper, IDropdownHelper dropdownHelper, AppDbContext context, IEmailHelper emailHelper)
        {
            _userHelper = userHelper;
            _applicationHelper = applicationHelper;
            _signInManager = signInManager;
            _userManager = userManager;
            _adminHelper = adminHelper;
            _dropdownHelper = dropdownHelper;
            _context = context;
            _emailHelper = emailHelper;

        }

        public IActionResult Index()
        {
            var dashBoardData = _adminHelper.DashboardBuildingServices();
            return View(dashBoardData);
        }
        [HttpGet]
        public IActionResult OnboardingStudent()
        {
            var onboardingStudentList = _userHelper.GetAllOnboardApplicantsFromDB();
            return View(onboardingStudentList);
        }

        // POST ACTION FOR ACCEPT & REJECT APPLICATION
        [HttpPost]
        public JsonResult ApplicationStatusPost(ApplicationUser applicantDetails)
        {
            try
            {
                if (applicantDetails == null)
                {
                    return Json(new { isError = true, msg = "Aborted" });
                }
                if (applicantDetails.Status == ApplicationStatus.Accepted)
                {
                    var applicant = _applicationHelper.AcceptSelectedApplication(applicantDetails);
                    if (applicant != null)
                    {
                        var removeUserFromRole = _userManager.RemoveFromRoleAsync(applicant, "Applicant").Result;
                        if (removeUserFromRole.Succeeded)
                        {
                            var addToRole = _userManager.AddToRoleAsync(applicant, "Student").Result;
                            if (addToRole.Succeeded)
                            {
                                return Json(new { isError = false, msg = "Application accepted successfully" });
                            }
                        }
                    }
                }
                else if (applicantDetails.Status == ApplicationStatus.Rejected)
                {
                    var applicant = _applicationHelper.RejectSelectedApplication(applicantDetails);
                    if (applicant != null)
                    {
                        return Json(new { isError = false, msg = "Application rejected successfully" });
                    }
                }
                return Json(new { isError = true, msg = "Failed" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetApplicationProfileById(string Id)
        {
            try
            {
                if (Id != null)
                {
                    var application = _userManager.FindByIdAsync(Id).Result;
                    return Json(application);
                }
                else
                {
                    return Json(new { isError = true, msg = "Not Found" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { isError = true, msg = "An unexpected error occured " + ex.Message });
            }
        }
        // TRAINING COST SETUP
        public async Task<IActionResult> TrainingCourse()
        {
            var username = User.Identity.Name;
            ViewBag.Layout = _applicationHelper.GetUserLayout(username).FirstOrDefault();
            var allTrainingCourse = _userHelper.GetAllTrainingCourseFromDB();
            return View(allTrainingCourse);
        }

        // POST ACTION FOR ALL TRAINING COST SETUP
        [HttpPost]
        public JsonResult TrainingCoursePost(string collectedTrainingData)
        {
            try
            {
                if (collectedTrainingData != null)
                {
                    var rawTrainingData = JsonConvert.DeserializeObject<TrainingCourseViewModel>(collectedTrainingData);

                    if (rawTrainingData.ActionType == Create_Training_Cost_ActionType)
                    {
                        if (rawTrainingData.Title == null)
                        {
                            return Json(new { isError = true, msg = "Enter Course title" });
                        }
                        if (rawTrainingData.Description == null)
                        {
                            return Json(new { isError = true, msg = "Enter Course Description" });
                        }
                        if (rawTrainingData.Amount == null)
                        {
                            return Json(new { isError = true, msg = "Enter Course Amount" });
                        }
                        if (rawTrainingData.Duration == null)
                        {
                            return Json(new { isError = true, msg = "Enter Course Duration" });
                        }
                        if (rawTrainingData.TestDuration == null)
                        {
                            return Json(new { isError = true, msg = "Enter Course Test Duration in Minutes" });
                        }
                        if (rawTrainingData.Logo == null)
                        {
                            return Json(new { isError = true, msg = "Insert Course Icon" });
                        }
                        else
                        {
                            var newTrainingCourse = _adminHelper.AddTrainignCostServices(rawTrainingData);
                            if (newTrainingCourse != null)
                            {
                                return Json(new { isError = false, msg = "Added Successfully" });
                            }
                        }

                    }
                    else if (rawTrainingData.ActionType == Edit_Training_Cost_ActionType)
                    {
                        if (rawTrainingData.Title == null)
                        {
                            return Json(new { isError = true, msg = "Enter Course title" });
                        }
                        if (rawTrainingData.Description == null)
                        {
                            return Json(new { isError = true, msg = "Enter Course Description" });
                        }
                        if (rawTrainingData.Amount == null)
                        {
                            return Json(new { isError = true, msg = "Enter Course Amount" });
                        }
                        if (rawTrainingData.Duration == null)
                        {
                            return Json(new { isError = true, msg = "Enter Course Duration" });
                        }
                        if (rawTrainingData.Logo == null)
                        {
                            return Json(new { isError = true, msg = "Insert Course Icon" });
                        }
                        else
                        {
                            var costToEdit = _adminHelper.EditTrainignCostServices(rawTrainingData);
                            if (costToEdit != null)
                            {
                                return Json(new { isError = false, msg = "Update Successfully" });
                            }

                        }
                    }
                    else if (rawTrainingData.ActionType == Activate_Training_Cost_ActionType)
                    {

                        if (rawTrainingData.Id != null)
                        {
                            var costToActivate = _adminHelper.ActivateTrainignCostServices(rawTrainingData);
                            if (costToActivate != null)
                            {
                                return Json(new { isError = false, msg = "Activated Successfully" });
                            }
                        }

                    }
                    else if (rawTrainingData.ActionType == Deactivate_Training_Cost_ActionType)
                    {
                        if (rawTrainingData.Id != null)
                        {
                            var costToDisable = _adminHelper.DisableTrainignCostServices(rawTrainingData);
                            if (costToDisable != null)
                            {
                                return Json(new { isError = false, msg = "Deactivated Successfully" });
                            }
                        }
                    }
                    else if (rawTrainingData.ActionType == Delete_Training_Cost_ActionType)
                    {
                        if (rawTrainingData.Id != null)
                        {
                            var costToDelete = _adminHelper.DeleteTrainignCostServices(rawTrainingData);
                            if (costToDelete != null)
                            {
                                return Json(new { isError = false, msg = "Deleted Successfully" });
                            }
                        }
                    }
                }
                return Json(new { isError = true, msg = "Failed" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        // TRAINING COST SETUP  GET ACTION
        [HttpGet]
        public JsonResult GetTrainingCourseById(int? Id)
        {
            try
            {
                if (Id != null)
                {
                    var editedTrainingCourse = _userHelper.GetTrainingCourseById(Id);
                    return Json(editedTrainingCourse);
                }
                return Json(new { isError = true, msg = "Failed" });

            }
            catch (Exception ex)
            {

                return Json(new { isError = true, msg = "An unexpected error occured " + ex.Message });
            }
        }

        [HttpGet]
        public IActionResult Payments()
        {
            var username = User.Identity.Name;
            ViewBag.Layout = _applicationHelper.GetUserLayout(username).FirstOrDefault();
            var payment = _userHelper.GetPaymentList();
            return View(payment);
        }
        [HttpGet]
        public JsonResult GetCourseDescriptionById(int? id)
        {
            try
            {
                if (id != null)
                {
                    var courseDescription = _userHelper.GetTrainingCourseById(id).Description;
                    return Json(courseDescription);
                }
                return Json(new { isError = true, msg = "Failed" });

            }
            catch (Exception ex)
            {
                return Json(new { isError = true, msg = "An unexpected error occured " + ex.Message });
            }
        }

        // PAYMENT  GET ACTION
        [HttpGet]
        public JsonResult GetPaymentById(int? Id)
        {
            try
            {
                if (Id != null)
                {
                    var payment4Action = _userHelper.GetPaymentById(Id);
                    return Json(payment4Action);
                }
                return Json(new { isError = true, msg = "Failed" });

            }
            catch (Exception ex)
            {

                return Json(new { isError = true, msg = "An unexpected error occured " + ex.Message });
            }
        }

        // POST ACTION FOR ALL TRAINING COST SETUP
        [HttpPost]
        public JsonResult PaymentPostAction(string collectedPaymentID)
        {
            try
            {
                if (collectedPaymentID != null)
                {
                    var payment = JsonConvert.DeserializeObject<Payments>(collectedPaymentID);

                    if (payment.Status == PaymentStatus.Approved)
                    {

                        if (payment.Id > 0)
                        {
                            var paymentActivate = _adminHelper.ApproveSelectedPayment(payment);
                            if (paymentActivate != null)
                            {
                                return Json(new { isError = false, msg = "Approved Successfully" });
                            }
                        }

                    }
                    else if (payment.Status == PaymentStatus.Declined)
                    {
                        if (payment.Id != null)
                        {
                            var paymentToDecline = _adminHelper.DeclineSelectedPaymment(payment);
                            if (paymentToDecline != null)
                            {
                                return Json(new { isError = false, msg = "Declined Successfully" });
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

        [HttpGet]
        public async Task<IActionResult> TrainingVideos()
        {
            var username = User.Identity.Name;
            ViewBag.Layout = _applicationHelper.GetUserLayout(username).FirstOrDefault();
            var model = new TrainingVideosViewModel();
            ViewBag.AllCourses = _dropdownHelper.DropdownOfCourses();
            model.Videos = _userHelper.GetTrainingVideos();
            return View(model);
        }
    }
}
