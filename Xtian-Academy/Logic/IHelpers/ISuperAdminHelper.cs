using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.IHelpers
{
    public interface ISuperAdminHelper
    {
        List<ApplicationUser> GetListOfAllAdmin();
        ApplicationUser DeactivateAdminAccount(ApplicationUser collectedData);
        ApplicationUser ActivateAdminAccount(ApplicationUser collectedData);
    }
}
