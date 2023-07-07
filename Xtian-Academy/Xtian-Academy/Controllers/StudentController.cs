using Core.Database;
using Core.Models;
using Logic.IHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace Academy_App.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IDropdownHelper _dropdownHelper;
        private readonly IApplicationHelper _applicationHelper;
        private readonly IUserHelper _userHelper;
        private readonly IStudentHelper _studentHelper;


        public StudentController(AppDbContext context, UserManager<ApplicationUser> userManger, SignInManager<ApplicationUser> signInManager, IDropdownHelper dropdownHelper, 
            IApplicationHelper applicationHelper, IUserHelper userHelper, IStudentHelper studentHelper)
        {
            _context = context;
            _userManger = userManger;
            _signInManager = signInManager;
            _dropdownHelper = dropdownHelper;
            _applicationHelper = applicationHelper;
            _userHelper = userHelper;
            _studentHelper = studentHelper;
        }
        public IActionResult Index()
        {
            ViewBag.ApplicantDocumentRecordsExist = _studentHelper.DeosApplicantDocummentExist(User.Identity.Name);
            var dashBoardData = _studentHelper.DashboardBuildingServices(User.Identity.Name);
            return View(dashBoardData);
        }
        //Applicants GET ACTION
        [HttpGet]
        public IActionResult ApplicantsDocumentation()
        {
            var ApplicantDocumentRecordsExist = _studentHelper.DeosApplicantDocummentExist(User.Identity.Name);
            if (ApplicantDocumentRecordsExist)
            {
                return RedirectToAction("EditApplicantDocuments");
            }
            return View();
        }
        //Applicants POST ACTION
        [HttpPost]
        public JsonResult SubmitApplicantDocument(string applicantDetailedDocuments)
        {
            try
            {
                if (applicantDetailedDocuments != null)
                {
                    var loggedInUsername = User.Identity.Name;
                    var fullStudentDetails = _userManger.FindByNameAsync(loggedInUsername).Result;
                    var allUploadedDocuments = JsonConvert.DeserializeObject<ApplicantDocumment>(applicantDetailedDocuments);

                    if(allUploadedDocuments != null)
                    {
                        var result = _studentHelper.UpdateApplicantDocumentService(allUploadedDocuments, loggedInUsername);
                        if (result != null)
                        {
                            return Json(new { isError = false, msg = "Documents Uploaded Successfully." });
                        }
                    }
                }
                return Json(new { isError = true, msg = "Upload Failed" });

            }
            catch (Exception ex)
            {

                return Json(new { isError = true, msg = "An unexpected error occured " + ex.Message });
            }

        }

        [HttpGet]
        public IActionResult EditApplicantDocuments()
        {
            var user = User.Identity.Name;
            var userId = _userHelper.FindByUserNameAsync(user).Result.Id;
            {
                var applicantDetail = _userHelper.GetApplicationDocummentByUserId(userId);
                if (applicantDetail != null)
                {
                    return View(applicantDetail);
                }
                return View();
            }

        }
        [HttpPost]
        public JsonResult EditApplicantDocuments(string applicantDocuments)
        {
            try
            {
                var applicantNewDocumentDetails = JsonConvert.DeserializeObject<ApplicantDocumment>(applicantDocuments);
                if (applicantNewDocumentDetails != null)
                {
                    var updateApplicantNewDetails = _studentHelper.UpdateApplicantDocumentsInfo(applicantNewDocumentDetails);
                    if (updateApplicantNewDetails != null)
                    {
                        return Json(new { isError = false, msg = "Documents Edited Successfully." });
                    }
                }
                return Json(new { isError = true, msg = "Something went wrong." });
            }
            catch (Exception ex)
            {
                return Json(new { isError = true, msg = "An unexpected error occured " + ex.Message });
            }
        }

        //GET || Students course List
        [HttpGet]
        public IActionResult StudentCourses()
        {
            var userId = _userHelper.FindByUserNameAsync(User.Identity.Name).Result?.Id;
            if (userId != null)
            {
                ViewBag.PaidCourseIdList = _userHelper.GetListOfCourseIdStudentPaid4(userId);
            }
            var allCourses = _studentHelper.GetAllTrainingCourseDB();
            if (allCourses != null)
            {
                return View(allCourses);
            }
            return View();
        }
        [HttpGet]
        public JsonResult GetCourseOutLineById(Guid id)
        {
            try
            {
                if (id != Guid.Empty)
                {
                    var courseOutLine = _userHelper.GetVideosById(id).Outline;
                    return Json(courseOutLine);
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
