using Core.Config;
using Core.Database;
using Core.Models;
using Logic.IHelpers;
using Logic.SmtpMailServices;

namespace Logic.Helpers
{
    public class EmailHelper: IEmailHelper
    {
        private readonly IUserHelper _userHelper;
        private readonly IEmailService _emailService;
        private readonly AppDbContext _context;
        private readonly IGeneralConfiguration _generalConfiguration;

        public EmailHelper(AppDbContext context, IUserHelper userHelper, IEmailService emailService, IGeneralConfiguration generalConfiguration)
        {
            _context = context;
            _emailService = emailService;
            _userHelper = userHelper;
            _generalConfiguration = generalConfiguration;
        }

        public bool SendMailToApplicant(ApplicationUser userDetail, string linkToClick)
        {
            string toEmail = userDetail.Email;
            string subject = "Registration Completed Successfully";
            string message = "Dear " + userDetail.FirstName + ", You have successfully completed the registration in our platform" + " Please click on the link below to choose your interview date" + "<br>" +
                   "<a  href='" + linkToClick + "?userId=" + userDetail.Id + "' target='_blank'>" + "<button style='color:white; background-color:#018DE4; padding:10px; border:10px;'>Create Now</button>" + "</a>";
            _emailService.SendEmail(toEmail, subject, message);
            return true;
        }



        public bool VerificationEmail(string applicantEmail, string linkToClick)
        {
            try
            {
                if (applicantEmail != null)
                {
                    string toEmail = applicantEmail;
                    string subject = "Verify your account - Bivisoft Academy ";
                    string message = "Thank you for registering! You’re only one click away from accessing your Bivisoft Academy account." + "<br/>" + "<br/>" +
                        "<a  href='" + linkToClick + "' target='_blank'>" + "<button style='color:white; background-color:#FFA76F; padding:10px; border:10px;'>Verify Me Now</button>" + "</a>" + "<br/>" +
                        "If the link does not work copy the link here to your browser: " + linkToClick + "<br/>" +
                        "Need help? We’re here for you.Simply reply to this email to contact us. <br/>" +

                        "Kind regards,<br/>" +
                        "Bivisoft Limited Group.";
                    _emailService.SendEmail(toEmail, subject, message);
                    return true;
                };
                return false;
            }

            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserVerification> CreateUserToken(string userEmail)
        {
            try
            {
                var user = await _userHelper.FindByEmailAsync(userEmail);
                if (user != null)
                {
                    UserVerification userVerification = new UserVerification()
                    {
                        //UserId = user.Id,
                    };
                    await _context.AddAsync(userVerification);
                    await _context.SaveChangesAsync();
                    return userVerification;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

		public bool ChangePasswordAlert(ApplicationUser userDetail, string loginLink)
		{
			string toEmail = userDetail.Email;
			string subject = "PASSWORD CHANGE ALERT";
			string message = "Dear " + userDetail.FirstName + "," + "<br>" + "<br>" + "Your password has been changed successfully. If you did not make this change please email us at agbo4chris@yahoo.com" + "<br>" + "<br>" + "Regards";
			_emailService.SendEmail(toEmail, subject, message);

			ChangePasswordMailTemlate(userDetail, loginLink);
			return true;
		}

		public bool ChangePasswordMailTemlate(ApplicationUser userDetail, string loginLink)
		{
			string toEmail = userDetail.Email;
			string subject = "PASSWORD CHANGED SUCCESSFULLY ";
			string message = "Hi " + userDetail.FirstName + ", " + "<br>" + "<br>" + "You have successfully changed your password. please click on the link below to login with your new password" + "<br>" + "<br>" + "Regards" + "<br>" + "<br>" +
				   "<a  href='" + loginLink + "' target='_blank'>" + "<button style='color:white; background-color:#018DE4; padding:10px; border:10px;'>Login</button>" + "</a>";
			_emailService.SendEmail(toEmail, subject, message);
			return true;
		}

        public async void ForgotPasswordTemplateEmailer(ApplicationUser userEmail, string linkToClick)
        {
            var getUserId = await _userHelper.FindByUserNameAsync(userEmail.FirstName);
            if (userEmail != null)
            {
                string toEmail = userEmail.Email;
                string subject = "Reset your Email - Xtian Academy ";
                string message = "Hi!" + " " + userEmail.FirstName + "," + "<br/>" + "<br/>" +
                    "You requested for a password reset. please click on the link below to create a new password," + "<br/>" +
                    "<a  href='" + linkToClick + "' target='_blank'>" + "<button style='color:white; background-color:blue; padding:10px; border:10px;'>Password Reset</button>" + "</a>" + "<br/>" +
                    "If the link does not work copy the link here to your browser" + "<br/>" +
                    "<a  href='" + linkToClick + "' target='_blank'>" + linkToClick + "</a>" + "<br/>" +
                    "Need help? We’re here for you.Simply reply to this email to contact us. <br/>" +

                    "Kind regards,<br/>" +
                    "Xtian Limited Group.";
                _emailService.SendEmail(toEmail, subject, message);

            }

        }

        public async Task PasswordResetedTemplateEmailerAsync(ApplicationUser userEmail, string linkToClick)
        {
            var getUserId = await _userHelper.FindByUserNameAsync(userEmail.FirstName);
            if (userEmail != null)
            {
                string toEmail = userEmail.Email;
                string subject = "Reset your Email - Xtian Academy ";
                string message = "Hi!" + userEmail.FirstName + "," + "<br/>" + "<br/>" +
                "You requested for a password reset." + "Your password has been changed.If you did not make this change please email us at support@safecash.com<br> Regards" + " < br/>" +
                    "<a  href='" + linkToClick + "' target='_blank'>" + "<button style='color:white; background-color:#FFA76F; padding:10px; border:10px;'>Verify Me Now</button>" + "</a>" + "<br/>" +
                   "If you have any trouble with your account, you can always email us at agbo4chris@yahoo.com," + "<br/>" +
                    "Need help? We’re here for you.Simply reply to this email to contact us. <br/>" +

                    "Kind regards,<br/>" +
                    "Xtian Limited Group.";
                _emailService.SendEmail(toEmail, subject, message);

            }
        }

        public bool Gratitude(string applicantEmail)
        {
            try
            {

                if (applicantEmail != null)
                {

                    string toEmail = applicantEmail;
                    string subject = "Email Verified Successfully";
                    string message = "Your Email has been verified.Thank you for applying with Bivisoft Academy.<br/> " +
                        "<br/>" + "<br/>" + "Need help? We’re always here for you.Simply reply to this email to reach us. <br/>" + "<br/>" +
                    "Kind regards,<br/>" +
                    "<b>Bivisoft Limited.</b>";
                    _emailService.SendEmail(toEmail, subject, message);
                    return true;
                };

                return false;
            }

            catch (Exception)
            {
                throw;
            }
        }

    }
}
