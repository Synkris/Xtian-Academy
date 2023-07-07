using Core.Models;
using Core.ViewModels;

namespace Logic.IHelpers
{
    public interface IApplicationHelper
    {
        Task<ApplicationUser> RegisterApplicantService(ApplicationUserViewModel applicationUserViewModel);
        Task<ApplicationUser> AuthenticateUser(LoginViewModel loginDetail);
        string GetUserDashboardPage(ApplicationUser userr);
        Task<bool> LogOut();
        string GetUserLayout(string username);
        Task<ApplicationUser> RegisterAdminService(ApplicationUserViewModel applicationUserViewModel);
        ApplicationUser AcceptSelectedApplication(ApplicationUser applicantIdToAccept);
        ApplicationUser RejectSelectedApplication(ApplicationUser applicantIdToReject);


    }
}
