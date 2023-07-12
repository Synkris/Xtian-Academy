using Core.Enum;
using Core.Models;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic.IHelpers
{
	public interface IStudentHelper
	{
        ApplicantDocumment UpdateApplicantDocumentService(ApplicantDocumment applicantDetailsForUpdate, string username);
        
        ApplicantDocumment UpdateApplicantDocumentsInfo(ApplicantDocumment applicantDocuments);
        bool DeosApplicantDocummentExist(string username);
        StudentDashBoardViewModel DashboardBuildingServices(string userName);
        List<TrainingCourse> GetAllTrainingCourseDB();
        List<ProjectTopic> GetListOfStutentsProjectTopic(string userId);
        ProjectTopic UploadProjectTopicServices(ProjectTopicViewModel topics, string userId);
        List<ProjectTopic> GetListOfStutentsApprovedProjectTopic(string userId);
        ProjectTopic ProjectLinksUpdateServices(ProjectTopicViewModel topics);
        ProjectTopic GetProjectLinksServices(int id);
        bool CheckIfUserIsQualifiedToApplyForJobs(string userId);
        List<int> GetListOfJobIdsUserHaveAppliedFor(string userID);
        bool CheckIfUserHaveAppliedForThisJob(List<int> getListJobId, int id);
        List<JobViewModels> GetListOfAvailableJobsByJobType(string userID, JobType? id);
        List<JobViewModels> GetListOfAvailableJobs(string userID);
        Task<JobApplication> JobApplicationServices(int jobId, ApplicationUser user);


    }
}
