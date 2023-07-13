﻿using Core.Database;
using Core.Enum;
using Core.Models;
using Core.ViewModels;
using Logic.IHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Logic.Helpers
{
    public class StudentHelper: IStudentHelper
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IEmailHelper _emailHelper;

        public StudentHelper(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, AppDbContext context, IUserHelper userHelper, IEmailHelper emailHelper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _userHelper = userHelper;
            _emailHelper = emailHelper;
        }

        public ApplicantDocumment UpdateApplicantDocumentService(ApplicantDocumment applicantDetailsForUpdate, string username)
        {
            var student = _userManager.FindByNameAsync(username).Result;

            var applicantSubmittedDocument = new ApplicantDocumment
            {
                StudentId = student.Id,
                BVN = applicantDetailsForUpdate.BVN,
                FirstGuarantor = applicantDetailsForUpdate.FirstGuarantor,
                SecondGuarantor = applicantDetailsForUpdate.SecondGuarantor,
                NepaBill = applicantDetailsForUpdate.NepaBill,
                SignedContract = applicantDetailsForUpdate.SignedContract,
                IsDeleted = false,
            };
            if (applicantSubmittedDocument != null)
            {
                _context.ApplicantDocumments.Update(applicantSubmittedDocument);
                _context.SaveChanges();
                return applicantSubmittedDocument;
            }
            return null;
        }

        public ApplicantDocumment UpdateApplicantDocumentsInfo(ApplicantDocumment applicantDocuments)
        {
            try
            {
                if (applicantDocuments != null)
                {
                    var studentDetails = _context.ApplicantDocumments.Where(d => d.Id == applicantDocuments.Id).FirstOrDefault();
                    if (studentDetails != null)
                    {
                        studentDetails.BVN = applicantDocuments.BVN;
                        studentDetails.FirstGuarantor = applicantDocuments.FirstGuarantor;
                        studentDetails.SecondGuarantor = applicantDocuments.SecondGuarantor;
                        studentDetails.NepaBill = applicantDocuments.NepaBill;
                        studentDetails.SignedContract = applicantDocuments.SignedContract;
                        studentDetails.IsDeleted = false;

                        _context.Update(studentDetails);
                        _context.SaveChanges();
                        return studentDetails;
                    }
                    return null;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<ApplicantDocumment> GetAllApplicantsDocumentFromDB()
        {
            var allApplicantsDocuments = _context.ApplicantDocumments.Where(b => !b.IsDeleted).ToList();
            if (allApplicantsDocuments.Any())
            {
                return allApplicantsDocuments;
            }
            return allApplicantsDocuments;
        }

        public bool DeosApplicantDocummentExist(string username)
        {
            if (username != null)
            {
                var loggedInstudentDetails = _userManager.Users.Where(s => s.UserName == username).FirstOrDefault();
                var applicantHasApplied = _context.ApplicantDocumments.Where(x => x.StudentId == loggedInstudentDetails.Id).Any();
                if (applicantHasApplied)
                {
                    return true;
                }
            }
            return false;

        }

        /// <summary>
        /// Building up all the content in students dashboard
        /// </summary>
        /// <param name="userName">DashboardBuildingServices</param>
        /// <returns>StudentDashBoardViewModel</returns>
        public StudentDashBoardViewModel DashboardBuildingServices(string userName)
        {
            var adminDashboardData = new StudentDashBoardViewModel();
            if (userName != null)
            {
                var loggedInUser = _userHelper.FindByUserNameAsync(userName).Result;
                adminDashboardData.Users = loggedInUser;

                return adminDashboardData;
            }
            return adminDashboardData;
        }
        public List<TrainingCourse> GetAllTrainingCourseDB()
        {
            var allTrainingCourse = _context.TrainingCourse.Where(t => !t.IsDeleted && t.IsActive).ToList();
            if (allTrainingCourse.Any())
            {
                return allTrainingCourse;
            }
            return allTrainingCourse;
        }

        public List<ProjectTopic> GetListOfStutentsProjectTopic(string userId)
        {
            var allStutentsProjectTopic = _context.ProjectTopics.Where(p => p.UserId == userId).Include(c => c.Course).ToList();
            if (allStutentsProjectTopic.Any())
            {
                return allStutentsProjectTopic;
            }
            return null;
        }

        public ProjectTopic UploadProjectTopicServices(ProjectTopicViewModel topics, string userId)
        {
            if (topics != null)
            {
                var topicstDetails = new ProjectTopic
                {
                    UserId = userId,
                    CourseId = topics.CourseId,
                    Title = topics.Title,
                    Description = topics.Description,
                    IsApproved = false,
                    LinksIsAdded = false,
                    Status = ProjectStatus.New,
                    DatePosted = DateTime.Now

                };
                var newlyAddTopic = _context.ProjectTopics.Add(topicstDetails);
                _context.SaveChanges();
                if (newlyAddTopic != null)
                {
                    return topicstDetails;
                }
            }
            return null;

        }

        public List<ProjectTopic> GetListOfStutentsApprovedProjectTopic(string userId)
        {
            var allStutentsProjectTopic = _context.ProjectTopics.Where(p => p.UserId == userId && p.IsApproved).Include(c => c.Course).ToList();
            if (allStutentsProjectTopic.Any())
            {
                return allStutentsProjectTopic;
            }
            return null;
        }

        public ProjectTopic ProjectLinksUpdateServices(ProjectTopicViewModel topics)
        {
            if (topics != null)
            {
                var myApprovedTopic = _userHelper.GetProjectTopicById(topics.Id);

                if (myApprovedTopic != null)
                {
                    myApprovedTopic.GitLink = topics.GitLink;
                    myApprovedTopic.RedmineLink = topics.RedmineLink;
                    myApprovedTopic.LinksIsAdded = true;
                    myApprovedTopic.Status = ProjectStatus.Started;

                    var result = _context.ProjectTopics.Update(myApprovedTopic);
                    _context.SaveChanges();

                    return myApprovedTopic;
                }
            }

            return null;
        }

        public ProjectTopic GetProjectLinksServices(int id)
        {
            if (id != 0)
            {
                var myLinks = _context.ProjectTopics.Where(t => t.Id == id).FirstOrDefault();
                if (myLinks != null)
                {
                    return myLinks;
                }
            }
            return null;

        }
        public bool CheckIfUserIsQualifiedToApplyForJobs(string userId)
        {
            if (userId != null)
            {
                var projectCompletetionCheck = _context.ProjectTopics.Where(t => t.UserId == userId && t.Status == ProjectStatus.Accepted).FirstOrDefault();
                {
                    if (projectCompletetionCheck != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public List<JobViewModels> GetListOfAvailableJobsByJobType(string userID, JobType? id)
        {
            var result = new List<JobViewModels>();
            if (userID != null && id != null && id > 0)
            {
                var availableJobs = _context.Jobs.Where(t => t.IsActive && t.Type == id).ToList();

                if (availableJobs.Any())
                {
                    var appliedChecker = GetListOfJobIdsUserHaveAppliedFor(userID);
                    result = availableJobs.Select(x => new JobViewModels()
                    {
                        Id = x.Id,
                        CompanyName = x.CompanyName,
                        Title = x.Title,
                        Salary = x.Salary,
                        Type = x.Type,
                        Description = x.Description,
                        Responsibilities = x.Responsibilities,
                        Requirements = x.Requirements,
                        IsActive = x.IsActive,
                        DateCreated = x.DateCreated.ToString("R"),
                        DescriptionList = _userHelper.SplitStringToList(x.Description),
                        Applied = CheckIfUserHaveAppliedForThisJob(appliedChecker, x.Id),
                    }).OrderByDescending(i => i.Id).ToList();
                    return result;
                }
                return result;
            }
            return result;
        }

        public List<int> GetListOfJobIdsUserHaveAppliedFor(string userID)
        {
            var getListJobId = _context.JobApplications.Where(t => t.UserId == userID).ToList();
            var appliedJobIDs = getListJobId.Select(x => x.JobId).ToList();
            if (appliedJobIDs.Any())
            {
                return appliedJobIDs;
            }
            return null;
        }

        public bool CheckIfUserHaveAppliedForThisJob(List<int> getListJobId, int id)
        {
            if (getListJobId != null && getListJobId.Contains(id))
            {
                return true;
            }
            return false;
        }

        public List<JobViewModels> GetListOfAvailableJobs(string userID)
        {
            var result = new List<JobViewModels>();
            var availableJobs = _context.Jobs.Where(t => t.IsActive).ToList();

            if (availableJobs.Any())
            {
                var appliedChecker = GetListOfJobIdsUserHaveAppliedFor(userID);
                result = availableJobs.Select(x => new JobViewModels()
                {
                    Id = x.Id,
                    CompanyName = x.CompanyName,
                    Title = x.Title,
                    Salary = x.Salary,
                    Type = x.Type,
                    Description = x.Description,
                    Responsibilities = x.Responsibilities,
                    Requirements = x.Requirements,
                    IsActive = x.IsActive,
                    DateCreated = x.DateCreated.ToString("R"),
                    DescriptionList = _userHelper.SplitStringToList(x.Description),
                    Applied = CheckIfUserHaveAppliedForThisJob(appliedChecker, x.Id),
                }).OrderByDescending(i => i.Id).ToList();
                return result;
            }
            return null;
        }
        public async Task<JobApplication> JobApplicationServices(int jobId, ApplicationUser user)
        {
            if (user != null && jobId > 0)
            {
                var jobApplication = new JobApplication
                {
                    UserId = user.Id,
                    JobId = jobId,
                    DateApplied = DateTime.Now,
                };

                var saveCheck = await _context.JobApplications.AddAsync(jobApplication);
                _context.SaveChanges();
                if (saveCheck != null)
                {
                    var jobDetails = _context.Jobs.Where(j => j.Id == jobId).FirstOrDefault();
                    _emailHelper.SendMailToAdminOnJobApplication(user, jobDetails);
                    return jobApplication;
                }
            }
            return null;
        }

        public async Task<Payments> UploadMaualPaymentProve(PaymentsViewModel prove, string userId)
        {
            if (prove != null)
            {
                var myPayment = new Payments
                {
                    Source = TransactionType.Transfer,
                    InvoiceNumber = Generate().ToString(),
                    ProveOfPayment = prove.ProveOfPayment,
                    UserId = userId,
                    CourseId = prove.Id,
                    Status = PaymentStatus.Pending,
                    Date = DateTime.Now,
                };

                var saveCheck = await _context.Payments.AddAsync(myPayment);
                _context.SaveChanges();
                if (saveCheck != null)
                {
                    return myPayment;
                }
            }
            return null;
        }
        public static int Generate()
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            return rand.Next(100000000, 999999999);
        }
    }
}
