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
		bool ChangePasswordAlert(ApplicationUser userDetail, string loginLink);
        Task PasswordResetedTemplateEmailerAsync(ApplicationUser userEmail, string linkToClick);
        void ForgotPasswordTemplateEmailer(ApplicationUser userEmail, string linkToClick);
        bool Gratitude(string applicantEmail);
        bool SendPaymentAprovalMsg(ApplicationUser userDetail, string course);
        bool SendPaymentDeclineMsg(ApplicationUser userDetail, string course);



    }
}
