using Core.Models;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.IHelpers
{
    public interface IUserHelper
    {
        string GetFullNameByUserNameAsync(string userName);
        Task<ApplicationUser> FindByUserNameAsync(string userName);
        Task<ApplicationUser> FindByPhoneNumberAsync(string phoneNumber);
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<ApplicationUser> FindByIdAsync(string Id);
        Task<UserVerification> CreateUserToken(string userEmail);
        Task<UserVerification> GetUserToken(Guid token);
        Task<bool> MarkTokenAsUsed(UserVerification userVerification);
        ApplicantDocumment GetApplicationDocummentByUserId(string userID);
        List<int> GetListOfCourseIdStudentPaid4(string userID);
        TrainingVideos GetVideosById(Guid Id);
        List<ApplicationUser> GetAllOnboardApplicantsFromDB();
        List<Payments> GetPaymentList();
        Payments GetPaymentById(int? Id);
        List<TrainingCourse> GetAllTrainingCourseFromDB();
        TrainingCourse GetTrainingCourseById(int? Id);
        List<TrainingVideos> GetTrainingVideos();
        ProjectTopic GetProjectTopicById(int? id);
        List<string> SplitStringToList(string dataString);
        Job GetJobById(int? Id);
        InterviewViewModel GetInterviewQuestions(string username);
        List<string> GetInterviewOptListByQuestionIds(int id);
        int GetInterviewTestDuration();
        bool CheckIfUserHaveTakenThisInterview(string userID);
        List<TrainingVideos> GetStudentPaidTrainingVideos(string userID);
        List<TestQuestionsViewModel> GetTestQuestionsForPage1(int? Id);
        List<TestQuestionsViewModel> GetTestQuestionsForPage2(int? Id);
        TestQuestions GetQuestionsById(int? Id);
        List<TestQuestions> GetTestQuestions();
        List<ProjectTopic> AllSubmenttedProjectTopic();
        ProjectTopic CheckIfATopicHasBeenApprovedForTheSelectedCourse(ProjectTopic proj2Approve);
        List<string> GetOptListByQuestionIds(int id);
        List<InterviewQuestions> GetInterviewTestQuestions();
        InterviewQuestions GetInterviewQuestionsById(int Id);


    }
}
