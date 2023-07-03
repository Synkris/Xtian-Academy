using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.IHelpers
{
    public interface IEmailHelper
    {
        bool SendMailToApplicant(ApplicationUser userDetail, string linkToClick);
        Task<UserVerification> CreateUserToken(string userEmail);
        bool VerificationEmail(string applicantEmail, string linkToClick);


    }
}
