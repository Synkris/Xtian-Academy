﻿using Core.Database;
using Core.Enum;
using Core.Models;
using Core.ViewModels;
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
        private readonly IPaystackHelper _paystackHelper;


        public StudentController(AppDbContext context, UserManager<ApplicationUser> userManger, SignInManager<ApplicationUser> signInManager, IDropdownHelper dropdownHelper, 
            IApplicationHelper applicationHelper, IUserHelper userHelper, IStudentHelper studentHelper, IPaystackHelper paystackHelper)
        {
            _context = context;
            _userManger = userManger;
            _signInManager = signInManager;
            _dropdownHelper = dropdownHelper;
            _applicationHelper = applicationHelper;
            _userHelper = userHelper;
            _studentHelper = studentHelper;
            _paystackHelper = paystackHelper;
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

        [HttpGet]
        public IActionResult ProjectTopics()
        {
            var userId = _userHelper.FindByUserNameAsync(User.Identity.Name).Result.Id;
            ViewBag.AllCourses = _dropdownHelper.DropdownOfCoursesWhereIsTested(User.Identity.Name);
            var myTopics = _studentHelper.GetListOfStutentsProjectTopic(userId);
            if (myTopics != null)
            {
                return View(myTopics);
            }
            return View();
        }
        [HttpPost]
        public JsonResult UploadProjectTopics(string topics)
        {
            if (topics != null)
            {
                var myTopics = JsonConvert.DeserializeObject<ProjectTopicViewModel>(topics);
                if (myTopics != null)
                {
                    var userId = _userHelper.FindByUserNameAsync(User.Identity.Name).Result.Id;
                    var newTopic = _studentHelper.UploadProjectTopicServices(myTopics, userId);
                    if (newTopic != null)
                    {
                        return Json(new { isError = false, msg = "Topic Uploaded Successfully." });
                    }
                }
            }
            return Json(new { isError = true, msg = "Upload Failed" });
        }
        [HttpGet]
        public IActionResult AprovedTopics()
        {
            var userId = _userHelper.FindByUserNameAsync(User.Identity.Name).Result.Id;
            var myTopics = _studentHelper.GetListOfStutentsApprovedProjectTopic(userId);
            if (myTopics != null)
            {
                return View(myTopics);
            }
            return View();
        }
        [HttpPost]
        public JsonResult ProjectLinkUpdatePost(string topicData)
        {
            if (topicData != null)
            {
                var myTopics = JsonConvert.DeserializeObject<ProjectTopicViewModel>(topicData);
                if (myTopics != null)
                {
                    var result = _studentHelper.ProjectLinksUpdateServices(myTopics);

                    if (result != null)
                    {
                        return Json(new { isError = false, msg = "Updated Successfully." });
                    }
                }
            }
            return Json(new { isError = true, msg = "Update Failed" });
        }
        [HttpGet]
        public JsonResult GetProjectLinksById(int id)
        {
            try
            {
                if (id != 0)
                {
                    var links = _studentHelper.GetProjectLinksServices(id);
                    return Json(links);
                }
                return Json(new { isError = true, msg = "Failed" });

            }
            catch (Exception ex)
            {
                return Json(new { isError = true, msg = "An unexpected error occured " + ex.Message });
            }
        }
        [HttpGet]
        public IActionResult Jobs(JobType? id)
        {
            ViewBag.JobTypes = _dropdownHelper.JobTypesForSearch();
            var userId = _userHelper.FindByUserNameAsync(User.Identity.Name).Result?.Id;
            var model = new JobViewModels();
            var projectCompletetionCheck = _studentHelper.CheckIfUserIsQualifiedToApplyForJobs(userId);
            {
                if (projectCompletetionCheck)
                {
                    //For Searching using job type
                    if (id > 0 && id != null)
                    {
                        var myJob = _studentHelper.GetListOfAvailableJobsByJobType(userId, id).OrderByDescending(i => i.Id).ToList();
                        if (myJob.Any())
                        {
                            model.JobModel = myJob;
                            model.Message = false;
                            model.IsProjectCompleted = true;
                        }
                        else
                        {
                            model.JobModel = _studentHelper.GetListOfAvailableJobs(userId).OrderByDescending(i => i.Id).ToList();
                            model.Message = true;
                            model.IsProjectCompleted = true;
                        }
                    }
                    else
                    {
                        model.JobModel = _studentHelper.GetListOfAvailableJobs(userId).OrderByDescending(i => i.Id).ToList();
                        model.Message = false;
                        model.IsProjectCompleted = true;
                    }
                    if (model != null)
                    {
                        return View(model);
                    }
                    return View();
                }
                else
                {
                    model.IsProjectCompleted = false;
                    return View(model);
                }
            }
        }

        public JsonResult JobApplicationPost(int id)
        {
            if (id > 0)
            {
                var user = _userHelper.FindByUserNameAsync(User.Identity.Name).Result;
                var paymentData = _studentHelper.JobApplicationServices(id, user);
                if (paymentData != null)
                {
                    return Json(new { isError = false, msg = "Application Submitted Successfully" });
                }
            }
            return Json(new { isError = true, msg = "Error occured! Please try again" });
        }
        [HttpGet]
        public JsonResult FindJob(int jobId)
        {
            try
            {
                if (jobId != null)
                {
                    var job4Action = _userHelper.GetJobById(jobId);
                    return Json(job4Action);
                }
                return Json(new { isError = true, msg = "Failed" });
            }
            catch (Exception ex)
            {

                return Json(new { isError = true, msg = "An unexpected error occured " + ex.Message });
            }
        }
        public JsonResult GetPaymentDetials(int? id)
        {
            if (id != null)
            {
                var user = _userHelper.FindByUserNameAsync(User.Identity.Name).Result;
                var paymentData = _userHelper.GetTrainingCourseById(id);
                if (paymentData != null && user != null)
                {
                    var paymentLink = _paystackHelper.GeneratePaymentParameters(paymentData, user).Result;
                    if (paymentLink != null)
                    {
                        var paylink = paymentLink.data.authorization_url;
                        return Json(new { isError = false, paystackUrl = paylink });
                    }
                }
            }
            return Json(new { isError = true, msg = "Failed" });
        }
        [AllowAnonymous]
        public IActionResult PaystackResponseFeedback(Paystack paystack)
        {
            if (paystack != null)
            {
                var isCoursePayment = _paystackHelper.GetPaystackHistoryByReference(paystack);
                if (isCoursePayment != null)
                {
                    var paymentResponse = _paystackHelper.GetPaymentResponse(isCoursePayment);
                    if (paymentResponse)
                    {
                        return RedirectToAction("StudentCourses");
                    }
                }
                else
                {
                    var isSalaryPayment = _paystackHelper.GetReoccuringPaymentsHistoryByReference(paystack);
                    if (isSalaryPayment != null)
                    {
                        var paymentResponse = _paystackHelper.CreateSalaryPaymentHistory(paystack);
                        if (paymentResponse)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }
            return RedirectToAction("Login", "Accounts");
        }
        [HttpPost]
        public JsonResult ManualPaymenUpload(string paymentData)
        {
            if (paymentData != null)
            {
                var myPaymentProve = JsonConvert.DeserializeObject<PaymentsViewModel>(paymentData);
                if (myPaymentProve != null)
                {
                    var userId = _userHelper.FindByUserNameAsync(User.Identity.Name).Result.Id;
                    //var proveSaved = _studentHelper.UploadMaualPaymentProve(myPaymentProve, userId);
                    //if (proveSaved != null)
                    //{
                    //    return Json(new { isError = false, msg = "Uploaded Successfully. Your Upload will be reviewed before you can access this Course" });
                    //}
                }
            }
            return Json(new { isError = true, msg = "Upload Failed" });
        }
        public IActionResult GraduationForm()
        {
            return View();
        }

        [HttpPost]
        public JsonResult EmploymentInformationPost(string employmentData)
        {
            if (employmentData != null)
            {
                var employmentInfo = JsonConvert.DeserializeObject<EmployementDataViewModel>(employmentData);
                if (employmentInfo != null)
                {
                    var userId = _userHelper.FindByUserNameAsync(User.Identity.Name).Result.Id;
                    if (userId != null)
                    {
                        var employmentDetailsCreated = _studentHelper.EMploymentDataServer(employmentInfo, userId);
                        if (employmentDetailsCreated != null)
                        {
                            return Json(new { isError = false, msg = "Submitted Successfully." });
                        }
                    }
                }
            }
            return Json(new { isError = true, msg = "Failed" });
        }

        [HttpGet]
        public IActionResult InterviewQuestions()
        {
            var modelEmpty = new InterviewViewModel();
            var username = User.Identity.Name;
            if (username != null)
            {
                var model = _userHelper.GetInterviewQuestions(username);
                return View(model);
            }
            return View(modelEmpty);
        }

        public JsonResult SubmitInterviewAnwser(string collectedData)
        {
            try
            {
                if (collectedData != null)
                {
                    var user = User.Identity.Name;
                    var answerDetails = JsonConvert.DeserializeObject<InterviewQuestionsViewModel>(collectedData);
                    if (answerDetails != null)
                    {
                        var result = _studentHelper.InterviewAnswerComputation(answerDetails, user);
                        if (result != null)
                        {
                            return Json(new { isError = false, msg = "Submitted Succesfully. Admin will review and get back to you" });
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
        public IActionResult TrainingVideos()
        {
            var userId = _userHelper.FindByUserNameAsync(User.Identity.Name).Result?.Id;
            if (userId != null)
            {
                var videos = _userHelper.GetStudentPaidTrainingVideos(userId);
                return View(videos);
            }
            return View();
        }
        [HttpGet]
        public IActionResult TakeTest(int? id)
        {
            try
            {
                if (id != null)
                {
                    var userId = _userHelper.FindByUserNameAsync(User.Identity.Name).Result.Id;
                    var model = new TestResultViewModel();
                    model.Courses = _studentHelper.GetTrainingCourseById(id);
                    model.Result = _studentHelper.GetStudentResult(id, userId);
                    return View(model);
                }
                return Redirect("TrainingVideos");
            }
            catch (Exception ex)
            {

                return Json(new { isError = true, msg = "An unexpected error occured " + ex.Message });
            }

        }
        [HttpGet]
        public IActionResult FirstTest(int? id)
        {
            try
            {
                if (id != null)
                {
                    if (id != null)
                    {
                        var user = User.Identity.Name;
                        var model = new GeneralViewModel();
                        model.QuestionMain = _userHelper.GetTestQuestionsForPage1(id);
                        model.Courses = _studentHelper.GetTrainingCourseById(id);
                        return View(model);
                    }
                }
                return Redirect("TrainingVideos");
            }
            catch (Exception ex)
            {
                return Json(new { isError = true, msg = "An unexpected error occured " + ex.Message });
            }
        }
        public IActionResult SecondTest(int? id)
        {
            try
            {
                if (id != null)
                {
                    if (id != null)
                    {
                        var userId = _userHelper.FindByUserNameAsync(User.Identity.Name).Result.Id;
                        var model = new GeneralViewModel();
                        model.QuestionMain = _userHelper.GetTestQuestionsForPage2(id);
                        model.Courses = _studentHelper.GetTrainingCourseById(id);
                        model.TestResults = _studentHelper.GetStudentResult(id, userId);
                        return View(model);
                    }
                }
                return Redirect("TrainingVideos");
            }
            catch (Exception ex)
            {

                return Json(new { isError = true, msg = "An unexpected error occured " + ex.Message });
            }
        }
        public JsonResult SubmitTestQuestions(string collectedData)
        {
            try
            {
                if (collectedData != null)
                {
                    var user = User.Identity.Name;
                    var answerDetails = JsonConvert.DeserializeObject<TestQuestionsViewModel>(collectedData);
                    var result = _studentHelper.ListOfAnsweredQuestions(answerDetails, user);
                    if (result != null)
                    {
                        return Json(new { isError = false, msg = "Submitted Succesfully", courseID = result.CourseId });
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
