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


  
    }
}
