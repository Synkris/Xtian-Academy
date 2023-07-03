using Core.Models;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.IHelpers
{
    public interface IApplicationHelper
    {
        Task<ApplicationUser> RegisterApplicantService(ApplicationUserViewModel applicationUserViewModel);
        Task<ApplicationUser> AuthenticateUser(LoginViewModel loginDetail);
        string GetUserDashboardPage(ApplicationUser userr);
        Task<bool> LogOut();
        string GetUserLayout(string username);

	}
}
