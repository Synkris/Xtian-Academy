﻿using Core.Models;
using Core.ViewModels;

namespace Logic.IHelpers
{
    public interface IAdminHelper
    {
        AdminDashBoardViewModel DashboardBuildingServices();
        TrainingCourse AddTrainignCostServices(TrainingCourseViewModel collectedData);
        TrainingCourse EditTrainignCostServices(TrainingCourseViewModel collectedData);
        TrainingCourse DisableTrainignCostServices(TrainingCourseViewModel collectedData);
        TrainingCourse ActivateTrainignCostServices(TrainingCourseViewModel collectedData);
        TrainingCourse DeleteTrainignCostServices(TrainingCourseViewModel collectedData);
        Payments ApproveSelectedPayment(Payments paymentData);
        Payments DeclineSelectedPaymment(Payments paymentData);
        TrainingVideos TrainignVideoServices(TrainingVideosViewModel collectedData);

        TestQuestions TestQuestionsServices(TestQuestionsViewModel collectedData);
        List<string> GetOptListByQuestionIds(int id);
        AnswerOptions PostServices4Options(AnswerOptions collectedData);
        List<ProjectTopic> GetListOfAllApprovedProjectTopic();
        ProjectTopic ProjectCompletionServices(int topicId);
        List<Job> GetListOfAllJobs();
        Job JobManagementServices(JobViewModels collectedData);
        List<EmployementData> GetListOfEmploymentData();
        InterviewQuestions InterviewQuestionsServices(InterviewQuestionsViewModel collectedData);
        List<string> GetInterviewOptListByQuestionIds(int id);
        InterviewAnswerOptions PostServicesInterviewAnsOptions(InterviewAnswerOptions collectedData);
        List<SalaryRetures> GetListOfSalaryPaymentsHistory();
        bool AddCustomerSUbscriptionLink(ReocurringPaymentViewModel reocurringPaymentViewModel, int mainId);
        bool ManageGraduationFormServices(EmployementDataViewModel employementDataViewModel);
        ReoccuringPayments SaveCreatedPlan(ReocurringPaymentViewModel reocurringPaymentViewModel, string userId);
        int TotalActiveStudents();
        int TotalNumberOfStudentsProjects();
        int TotalNumberOfJobApplications();
        List<ProjectTopic> GetListOfApprovedProjects();

    }
}
