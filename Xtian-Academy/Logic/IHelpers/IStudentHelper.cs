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


    }
}
