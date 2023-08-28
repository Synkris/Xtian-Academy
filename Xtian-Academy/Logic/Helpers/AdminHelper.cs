using Core.Config;
using Core.Database;
using Core.Enum;
using Core.Models;
using Core.ViewModels;
using Logic.IHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Logic.Helpers
{
    public class AdminHelper : IAdminHelper
    {
        private readonly IUserHelper _userHelper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly IEmailHelper _emailHelper;
        private readonly IGeneralConfiguration _generalConfiguration;
        public AdminHelper(IUserHelper userHelper, UserManager<ApplicationUser> userManager, AppDbContext context, IEmailHelper emailHelper, IGeneralConfiguration generalConfiguration)
        {
            _userHelper = userHelper;
            _userManager = userManager;
            _context = context;
            _emailHelper = emailHelper;
            _generalConfiguration = generalConfiguration;
        }

        #region This handles Admin Dashboard
        /// <summary>
        /// This is where all the content in admin dashboard are built 
        /// </summary>
        /// <returns>AdminDashBoardViewModel</returns>
        public AdminDashBoardViewModel DashboardBuildingServices()
        {
            var adminDashboardData = new AdminDashBoardViewModel();
            adminDashboardData.TotalActiveStudents = TotalActiveStudents();
            adminDashboardData.TotalNumberOfStudentsProjects = TotalNumberOfStudentsProjects();
            adminDashboardData.TotalNumberOfJobApplications = TotalNumberOfJobApplications();
            adminDashboardData.OnboardStudentsList = _userHelper.GetAllOnboardApplicantsFromDB().Take(4).ToList();
            adminDashboardData.PaymentHistoryList = _userHelper.GetPaymentList().Take(5).ToList();
            adminDashboardData.ApprovedProjectList = GetListOfApprovedProjects();
            return adminDashboardData;

        }
        /// <summary>
        /// This gets the total number of activte students
        /// </summary>
        /// <returns> Int Total num bers of active students </returns>
        public int TotalActiveStudents()
        {
            return _context.ApplicationUser.Where(u => u.Id != null && !u.Deactivated && u.Status == ApplicationStatus.Accepted).Count();
        }
        /// <summary>
        /// This gets the total number of approved students projects
        /// </summary>
        /// <returns> Int Total num bers of approved projects </returns>
        public int TotalNumberOfStudentsProjects()
        {
            return _context.ProjectTopics.Where(p => p.Id > 0 && p.IsApproved).Count();
        }
        /// <summary>
        /// This gets the total number of jobs
        /// </summary>
        /// <returns> Int Total num bers of jobs </returns>
        public int TotalNumberOfJobApplications()
        {
            return _context.JobApplications.Where(p => p.Id > 0).Count();
        }
        /// <summary>
        /// Get the 5 list of the most recent approved projects
        /// </summary>
        /// <returns> List of approved projects </returns>
        public List<ProjectTopic> GetListOfApprovedProjects()
        {
            var allJobs = _context.ProjectTopics.Where(p => p.IsApproved && p.Status != ProjectStatus.New).OrderByDescending(d => d.DatePosted).Take(5).ToList();
            if (allJobs.Any())
            {
                return allJobs;
            }
            return null;
        }
        #endregion admin dashboard ends

        public TrainingCourse AddTrainignCostServices(TrainingCourseViewModel collectedData)
        {
            if (collectedData != null)
            {
                var newCost = new TrainingCourse
                {
                    Title = collectedData.Title,
                    Description = collectedData.Description,
                    Logo = collectedData.Logo,
                    Duration = collectedData.Duration,
                    TestDuration = collectedData.TestDuration,
                    Amount = Convert.ToDecimal(collectedData.Amount),
                    IsActive = true,
                    IsDeleted = false,
                    DateCreated = DateTime.Now,
                };
                _context.TrainingCourse.Add(newCost);
                _context.SaveChanges();

                return newCost;
            }
            return null;
        }
        public TrainingCourse EditTrainignCostServices(TrainingCourseViewModel collectedData)
        {
            var costToEdit = _context.TrainingCourse.Where(c => c.Id == collectedData.Id).FirstOrDefault();
            if (costToEdit != null)
            {
                costToEdit.Title = collectedData.Title;
                costToEdit.Description = collectedData.Description;
                costToEdit.Logo = collectedData.Logo;
                costToEdit.Duration = collectedData.Duration;
                costToEdit.TestDuration = collectedData.TestDuration;
                costToEdit.Amount = Convert.ToDecimal(collectedData.Amount);
                costToEdit.IsActive = true;

                _context.TrainingCourse.Update(costToEdit);
                _context.SaveChanges();

                return costToEdit;
            }
            return null;
        }
        public TrainingCourse DisableTrainignCostServices(TrainingCourseViewModel collectedData)
        {
            var costToTurnOff = _context.TrainingCourse.Where(c => c.Id == collectedData.Id).FirstOrDefault();
            if (costToTurnOff != null)
            {
                costToTurnOff.IsActive = false;

                _context.TrainingCourse.Update(costToTurnOff);
                _context.SaveChanges();

                return costToTurnOff;
            }
            return null;
        }
        public TrainingCourse ActivateTrainignCostServices(TrainingCourseViewModel collectedData)
        {
            var costToTurnOn = _context.TrainingCourse.Where(c => c.Id == collectedData.Id).FirstOrDefault();
            if (costToTurnOn != null)
            {
                costToTurnOn.IsActive = true;

                _context.TrainingCourse.Update(costToTurnOn);
                _context.SaveChanges();

                return costToTurnOn;
            }
            return null;
        }
        public TrainingCourse DeleteTrainignCostServices(TrainingCourseViewModel collectedData)
        {
            var costToDelete = _context.TrainingCourse.Where(c => c.Id == collectedData.Id).FirstOrDefault();
            if (costToDelete != null)
            {
                costToDelete.IsDeleted = true;

                _context.TrainingCourse.Update(costToDelete);
                _context.SaveChanges();

                return costToDelete;
            }
            return null;
        }
        //PAYMENT ACCEPT POST SERVICE
        public Payments ApproveSelectedPayment(Payments paymentData)
        {
            if (paymentData != null)
            {
                var paymentDetails = _userHelper.GetPaymentById(paymentData.Id);

                paymentDetails.Status = PaymentStatus.Approved;

                var approved = _context.Payments.Update(paymentDetails);
                _context.SaveChanges();

                if (approved != null)
                {
                    _emailHelper.SendPaymentAprovalMsg(paymentDetails.Student, paymentDetails.Courses.Title);
                }

                return paymentDetails;
            }
            else
            {
                return null;
            }
        }
        //PAYMENT DECLINE POST SERVICE
        public Payments DeclineSelectedPaymment(Payments paymentData)
        {
            if (paymentData != null)
            {
                var paymentDetails = _userHelper.GetPaymentById(paymentData.Id);

                paymentDetails.Status = PaymentStatus.Declined;

                var declined = _context.Payments.Update(paymentDetails);
                _context.SaveChanges();

                if (declined != null)
                {
                    _emailHelper.SendPaymentDeclineMsg(paymentDetails.Student, paymentDetails.Courses.Title);
                }

                return paymentDetails;
            }
            else
            {
                return null;
            }
        }
        public TrainingVideos TrainignVideoServices(TrainingVideosViewModel collectedData)
        {
            if (collectedData.ActionType == GeneralAction.CREATE)
            {
                var newVideo = new TrainingVideos
                {
                    CourseId = collectedData.CourseId,
                    VideoLink = collectedData.VideoLink,
                    Outline = collectedData.Outline,
                    IsActive = true,
                    DateCreated = DateTime.Now,
                };

                _context.TrainingVideos.Add(newVideo);
                _context.SaveChanges();

                return newVideo;
            }
            else if (collectedData.ActionType == GeneralAction.EDIT)
            {
                var videoEdited = _userHelper.GetVideosById(collectedData.Id);

                videoEdited.CourseId = collectedData.CourseId;
                videoEdited.VideoLink = collectedData.VideoLink;
                videoEdited.Outline = collectedData.Outline;

                _context.TrainingVideos.Update(videoEdited);
                _context.SaveChanges();

                return videoEdited;
            }
            else if (collectedData.ActionType == GeneralAction.DELETE)
            {
                var videoDeleted = _userHelper.GetVideosById(collectedData.Id);

                videoDeleted.IsActive = false;

                _context.TrainingVideos.Update(videoDeleted);
                _context.SaveChanges();

                return videoDeleted;
            }
            return null;
        }

        public TestQuestions TestQuestionsServices(TestQuestionsViewModel collectedData)
        {
            if (collectedData.ActionType == GeneralAction.CREATE)
            {
                var newQuestion = new TestQuestions
                {
                    CourseId = collectedData.CourseId,
                    Question = collectedData.Question,
                    Answer = collectedData.Answer,
                    IsActive = true,
                    IsDeleted = false,
                    DateCreated = DateTime.Now,
                };

                _context.TestQuestions.Add(newQuestion);
                _context.SaveChanges();

                return newQuestion;
            }
            else if (collectedData.ActionType == GeneralAction.EDIT)
            {
                var questionEdited = _userHelper.GetQuestionsById(collectedData.Id);

                questionEdited.CourseId = collectedData.CourseId;
                questionEdited.Question = collectedData.Question;
                questionEdited.Answer = collectedData.Answer;

                _context.TestQuestions.Update(questionEdited);
                _context.SaveChanges();

                return questionEdited;
            }
            else if (collectedData.ActionType == GeneralAction.ACTIVATE)
            {
                var questionActivate = _userHelper.GetQuestionsById(collectedData.Id);

                questionActivate.IsActive = true;

                _context.TestQuestions.Update(questionActivate);
                _context.SaveChanges();

                return questionActivate;
            }
            else if (collectedData.ActionType == GeneralAction.DEACTIVATE)
            {
                var questionDeactivate = _userHelper.GetQuestionsById(collectedData.Id);

                questionDeactivate.IsActive = false;

                _context.TestQuestions.Update(questionDeactivate);
                _context.SaveChanges();

                return questionDeactivate;
            }
            else if (collectedData.ActionType == GeneralAction.DELETE)
            {
                var questionDeleted = _userHelper.GetQuestionsById(collectedData.Id);

                questionDeleted.IsDeleted = true;

                _context.TestQuestions.Update(questionDeleted);
                _context.SaveChanges();

                return questionDeleted;
            }
            return null;
        }

        public List<string> GetOptListByQuestionIds(int id)
        {
            var optList = new List<string>();
            var optListDetails = _context.AnswerOptions.Where(a => a.QuestionId == id).OrderByDescending(o => o.Id).ToList();
            if (optListDetails.Any())
            {
                optList = optListDetails.Select(a => a.Option).ToList();
                return optList;
            }
            return null;
        }

        public AnswerOptions PostServices4Options(AnswerOptions collectedData)
        {
            if (collectedData != null)
            {
                var newOption = new AnswerOptions
                {
                    QuestionId = collectedData.Id,
                    Option = collectedData.Option,
                };

                _context.AnswerOptions.Add(newOption);
                _context.SaveChanges();

                return newOption;
            }
            return null;
        }

        public List<ProjectTopic> GetListOfAllApprovedProjectTopic()
        {
            var allApprovedProjectTopic = _context.ProjectTopics.Where(p => p.IsApproved).Include(c => c.Course).ToList();
            if (allApprovedProjectTopic.Any())
            {
                return allApprovedProjectTopic;
            }
            return null;
        }
        public ProjectTopic ProjectCompletionServices(int topicId)
        {
            if (topicId > 0)
            {
                var myCompletedProject = _userHelper.GetProjectTopicById(topicId);

                if (myCompletedProject != null)
                {
                    myCompletedProject.Status = ProjectStatus.Accepted;

                    var result = _context.ProjectTopics.Update(myCompletedProject);
                    _context.SaveChanges();

                    if (result != null)
                    {
                        _emailHelper.SendMailToStudentsOnProjectCompletion(myCompletedProject.Student.Email);
                        return myCompletedProject;
                    }
                }
            }

            return null;
        }
        public List<Job> GetListOfAllJobs()
        {
            var allJobs = _context.Jobs.OrderBy(d => d.DateCreated).ToList();
            if (allJobs.Any())
            {
                return allJobs;
            }
            return null;
        }
        public Job JobManagementServices(JobViewModels collectedData)
        {
            if (collectedData.ActionType == GeneralAction.CREATE)
            {
                var newJob = new Job
                {
                    CompanyName = collectedData.CompanyName,
                    Title = collectedData.Title,
                    Salary = collectedData.Salary,
                    Type = collectedData.Type,
                    Description = collectedData.Description,
                    Responsibilities = collectedData.Responsibilities,
                    Requirements = collectedData.Requirements,
                    IsActive = true,
                    DateCreated = DateTime.Now,
                };

                _context.Jobs.Add(newJob);
                _context.SaveChanges();

                return newJob;
            }
            else if (collectedData.ActionType == GeneralAction.EDIT)
            {
                var job = _userHelper.GetJobById(collectedData.Id);

                job.CompanyName = collectedData.CompanyName;
                job.Title = collectedData.Title;
                job.Salary = collectedData.Salary;
                job.Type = collectedData.Type;
                job.Description = collectedData.Description;
                job.Responsibilities = collectedData.Responsibilities;
                job.Requirements = collectedData.Requirements;

                _context.Jobs.Update(job);
                _context.SaveChanges();

                return job;
            }
            else if (collectedData.ActionType == GeneralAction.DEACTIVATE)
            {
                var job = _userHelper.GetJobById(collectedData.Id);

                job.IsActive = false;

                _context.Jobs.Update(job);
                _context.SaveChanges();

                return job;
            }
            else if (collectedData.ActionType == GeneralAction.ACTIVATE)
            {
                var job = _userHelper.GetJobById(collectedData.Id);

                job.IsActive = true;

                _context.Jobs.Update(job);
                _context.SaveChanges();

                return job;
            }
            return null;
        }
        public List<EmployementData> GetListOfEmploymentData()
        {
            var employmentList = _context.EmployementData.Include(u => u.User).OrderByDescending(d => d.DateCreated).ToList();
            if (employmentList.Any())
            {
                return employmentList;
            }
            return null;
        }
        public InterviewQuestions InterviewQuestionsServices(InterviewQuestionsViewModel collectedData)
        {
            if (collectedData.ActionType == GeneralAction.CREATE)
            {
                var newQuestion = new InterviewQuestions
                {
                    Question = collectedData.Question,
                    Answer = collectedData.Answer,
                    IsActive = true,
                    DateCreated = DateTime.Now,
                };

                _context.InterviewQuestions.Add(newQuestion);
                _context.SaveChanges();

                return newQuestion;
            }
            else if (collectedData.ActionType == GeneralAction.EDIT)
            {
                var questionEdited = _userHelper.GetInterviewQuestionsById(collectedData.Id);

                questionEdited.Question = collectedData.Question;
                questionEdited.Answer = collectedData.Answer;

                _context.InterviewQuestions.Update(questionEdited);
                _context.SaveChanges();

                return questionEdited;
            }
            else if (collectedData.ActionType == GeneralAction.ACTIVATE)
            {
                var questionActivate = _userHelper.GetInterviewQuestionsById(collectedData.Id);

                questionActivate.IsActive = true;

                _context.InterviewQuestions.Update(questionActivate);
                _context.SaveChanges();

                return questionActivate;
            }
            else if (collectedData.ActionType == GeneralAction.DEACTIVATE)
            {
                var questionDeactivate = _userHelper.GetInterviewQuestionsById(collectedData.Id);

                questionDeactivate.IsActive = false;

                _context.InterviewQuestions.Update(questionDeactivate);
                _context.SaveChanges();

                return questionDeactivate;
            }
            else if (collectedData.ActionType == GeneralAction.DELETE)
            {
                var questionDeleted = _userHelper.GetInterviewQuestionsById(collectedData.Id);

                _context.InterviewQuestions.Remove(questionDeleted);
                _context.SaveChanges();

                return questionDeleted;
            }
            return null;
        }
        public List<string> GetInterviewOptListByQuestionIds(int id)
        {
            var optList = new List<string>();
            var optListDetails = _context.InterviewAnswerOptions.Where(a => a.QuestionId == id).OrderByDescending(o => o.Id).ToList();
            if (optListDetails.Any())
            {
                optList = optListDetails.Select(a => a.Option).ToList();
                return optList;
            }
            return null;
        }
        public InterviewAnswerOptions PostServicesInterviewAnsOptions(InterviewAnswerOptions collectedData)
        {
            if (collectedData != null)
            {
                var newOption = new InterviewAnswerOptions
                {
                    QuestionId = collectedData.Id,
                    Option = collectedData.Option,
                };

                _context.InterviewAnswerOptions.Add(newOption);
                _context.SaveChanges();

                return newOption;
            }
            return null;
        }
        public List<SalaryRetures> GetListOfSalaryPaymentsHistory()
        {
            var history = new List<SalaryRetures>();

            var histories = _context.SalaryRetureHistory.Include(r => r.ReoccuringPayments).Include(r => r.ReoccuringPayments.User).OrderByDescending(d => d.PaymentDate).ToList();
            if (histories.Any())
            {
                return histories;
            }
            return history;
        }
        public bool ManageGraduationFormServices(EmployementDataViewModel employementDataViewModel)
        {
            try
            {
                if (employementDataViewModel != null)
                {
                    var employementData = _context.EmployementData.Where(e => e.Id == employementDataViewModel.Id).Include(u => u.User).FirstOrDefault();
                    if (employementData != null)
                    {
                        employementData.IsApproved = true;

                        _context.EmployementData.Update(employementData);
                        _context.SaveChanges();
                    }
                    var amt = Convert.ToInt32(employementData.MonthlyDeduction) * 100;
                    var upload = new UploadModel()
                    {
                        name = "Monthly Retainer",
                        interval = "monthly",
                        amount = Convert.ToString(amt),
                        invoice_limit = "24",
                    };
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _generalConfiguration.PayStakApiKey);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var uploadData = JsonConvert.SerializeObject(upload);
                        if (uploadData != null)
                        {
                            var content = new StringContent(uploadData, Encoding.Default, "application/json");
                            client.BaseAddress = new Uri("https://api.paystack.co");
                            var url = "/plan";
                            var result = client.PostAsync(url, content).Result;
                            if (result.IsSuccessStatusCode)
                            {
                                var response = result.Content.ReadAsStringAsync().Result;
                                var data = JsonConvert.DeserializeObject<ReocurringPaymentViewModel>(response);
                                if (data != null)
                                {
                                    var planCreated =  SaveCreatedPlan(data, employementData.UserId);
                                    if(planCreated != null)
                                    {
                                        var subscribe = new subscription()
                                        {
                                            email = employementData.User.Email,
                                            amount = Convert.ToString(planCreated.amount),
                                            plan = data.data.plan_code,
                                        };
                                        using (var checkout = new HttpClient())
                                        {
                                            checkout.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _generalConfiguration.PayStakApiKey);
                                            checkout.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                            var subscription = JsonConvert.SerializeObject(subscribe);
                                            if (subscription != null)
                                            {
                                                var subscriptionContent = new StringContent(subscription, Encoding.Default, "application/json");
                                                checkout.BaseAddress = new Uri("https://api.paystack.co");
                                                var subUrl = "/transaction/initialize";
                                                var subResult = checkout.PostAsync(subUrl, subscriptionContent).Result;
                                                if (subResult.IsSuccessStatusCode)
                                                {
                                                    var subResponse = subResult.Content.ReadAsStringAsync().Result;
                                                    var subData = JsonConvert.DeserializeObject<ReocurringPaymentViewModel>(subResponse);
                                                    if(subData != null)
                                                    {
                                                        var subscriptionUpdate = AddCustomerSUbscriptionLink(subData, planCreated.MainId);
                                                        if (subscriptionUpdate)
                                                        {
                                                            _emailHelper.SendMailToNewEmployee(subData.data.reference);
                                                            return true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    
                                }
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
          
        }

        public bool AddCustomerSUbscriptionLink(ReocurringPaymentViewModel reocurringPaymentViewModel, int mainId)
        {
            if (mainId > 0 && reocurringPaymentViewModel != null)
            {
                var subscriptionData = _context.ReoccuringPayments.Where(r => r.MainId == mainId).FirstOrDefault();
                if (subscriptionData != null)
                {
                    subscriptionData.dateUpdated = DateTime.Now;
                    subscriptionData.authorization_url = reocurringPaymentViewModel.data.authorization_url;
                    subscriptionData.access_code = reocurringPaymentViewModel.data.access_code;
                    subscriptionData.reference = reocurringPaymentViewModel.data.reference;

                    _context.ReoccuringPayments.Update(subscriptionData);
                    _context.SaveChanges();

                    return true;
                }
            }
            return false;
        }

        public ReoccuringPayments SaveCreatedPlan(ReocurringPaymentViewModel reocurringPaymentViewModel, string userId)
        {
            if (reocurringPaymentViewModel != null)
            {
                var saveData = new ReoccuringPayments()
                {
                    UserId = userId,
                    name = reocurringPaymentViewModel.data.name,
                    interval = reocurringPaymentViewModel.data.interval,
                    invoice_limit = reocurringPaymentViewModel.data.invoice_limit,
                    amount = (reocurringPaymentViewModel.data.amount / 100),
                    integration = reocurringPaymentViewModel.data.integration,
                    domain = reocurringPaymentViewModel.data.domain,
                    currency = reocurringPaymentViewModel.data.currency,
                    plan_code = reocurringPaymentViewModel.data.plan_code,
                    send_invoices = reocurringPaymentViewModel.data.send_invoices,
                    send_sms = reocurringPaymentViewModel.data.send_sms,
                    hosted_page = reocurringPaymentViewModel.data.hosted_page,
                    migrate = reocurringPaymentViewModel.data.migrate,
                    is_archived = reocurringPaymentViewModel.data.is_archived,
                    id = reocurringPaymentViewModel.data.id,
                    createdAt = reocurringPaymentViewModel.data.createdAt,
                    updatedAt = reocurringPaymentViewModel.data.updatedAt,
                    IsAuthorized = false,
                    dateUpdated = DateTime.Now,
                };
                _context.ReoccuringPayments.Add(saveData);
                _context.SaveChanges();
                return saveData;
            }
            return null;
        }

        public class UploadModel
        {
            public string name { get; set; }
            public string interval { get; set; }
            public string invoice_limit { get; set; }
            public string amount { get; set; }
        }

        public class subscription
        {
            public string email { get; set; }
            public string plan { get; set; }
            public string amount { get; set; }
        }


    }
}