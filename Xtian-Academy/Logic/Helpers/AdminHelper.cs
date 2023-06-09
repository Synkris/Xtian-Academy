﻿using Core.Config;
using Core.Database;
using Core.Enum;
using Core.Models;
using Core.ViewModels;
using Logic.IHelpers;
using Microsoft.AspNetCore.Identity;

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
 
    }
}