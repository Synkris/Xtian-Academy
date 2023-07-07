using Core.Models;
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

    }
}
