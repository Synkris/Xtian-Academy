using Core.Database;
using Core.Enum;
using Core.Models;
using Core.ViewModels;
using Logic.IHelpers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Helpers
{
    public class ApplicationHelper: IApplicationHelper
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDropdownHelper _dropDownHelper;
        private readonly IUserHelper _userHelper;
        private readonly AppDbContext _context;
        //private readonly IEmailHelper _emailHelper;
        private bool isPersistent;
        private bool lockoutOnFailure;


        public ApplicationHelper(UserManager<ApplicationUser> userManager, IDropdownHelper dropDownHelper, SignInManager<ApplicationUser> signInManager, IUserHelper userHelper, AppDbContext context)
        {
            _userManager = userManager;
            _dropDownHelper = dropDownHelper;
            _signInManager = signInManager;
            _userHelper = userHelper;
            _context = context;
        }
        public async Task<ApplicationUser> RegisterApplicantService(ApplicationUserViewModel applicationUserViewModel)
        {
            if (applicationUserViewModel != null)
            {
                var newInstanceOfApplicantModelAboutToBCreated = new ApplicationUser
                {
                    FirstName = applicationUserViewModel.FirstName,
                    LastName = applicationUserViewModel.LastName,
                    Email = applicationUserViewModel.Email,
                    PhoneNumber = applicationUserViewModel.PhoneNumber,
                    UserName = applicationUserViewModel.UserName,
                    Address = applicationUserViewModel.Address,
                    HasCompletedNysc = applicationUserViewModel.HasCompletedNysc,
                    HasLaptop = applicationUserViewModel.HasLaptop,
                    HasAnyProgrammingExp = applicationUserViewModel.HasAnyProgrammingExp,
                    ProgrammingLanguagesExps = applicationUserViewModel.ProgrammingLanguagesExps,
                    ApplicantResideInEnugu = applicationUserViewModel.ApplicantResideInEnugu,
                    ReasonForProgramming = applicationUserViewModel.ReasonForProgramming,
                    AboutYourSkills = applicationUserViewModel.AboutYourSkills,
                    HowDoYouIntendToCopeStatement = applicationUserViewModel.HowDoYouIntendToCopeStatement,
                    CV = applicationUserViewModel.CV,
                    Deactivated = false,
                    IsAdmin = false,
                    Status = ApplicationStatus.Pending,
                    DateRegistered = DateTime.Now,
                };
                var result = await _userManager.CreateAsync(newInstanceOfApplicantModelAboutToBCreated, applicationUserViewModel.Password);
                if (result.Succeeded)
                {
                    return newInstanceOfApplicantModelAboutToBCreated;
                }
            }
            return null;
        }

        public string GetUserDashboardPage(ApplicationUser userr)
        {
            var userRole = _userManager.GetRolesAsync(userr).Result.FirstOrDefault();
            if (userRole != null)
            {
                if (userRole == "SuperAdmin" || userRole == "Admin")
                {
                    return "/Admin/Index";

                }
                else
                {
                    return "/Student/Index";
                }
            }
            return null;
        }

        public async Task<ApplicationUser> AuthenticateUser(LoginViewModel loginDetail)
        {
            var user = await _userManager.FindByEmailAsync(loginDetail.Email);
            if (user != null && user.Deactivated != true)
            {
                var logger = _signInManager.PasswordSignInAsync(user.UserName, loginDetail.Password, isPersistent = true, lockoutOnFailure = false).Result;
                if (logger.Succeeded)
                {
                    return user;
                }
            }
            return null;
        }
        public async Task<bool> LogOut()
        {
            await _signInManager.SignOutAsync();
            return true;
        }

		public string GetUserLayout(string username)
		{
			var accountType = _userHelper.FindByUserNameAsync(username).Result;
			var userRole = _userManager.GetRolesAsync(accountType).Result.FirstOrDefault();
			if (userRole != null)
			{
				if (userRole == "SuperAdmin" || userRole == "Admin")
				{
					return "~/Views/Shared/_AdminLayout.cshtml";
				}
				else
				{
					return "~/Views/Shared/_StudentLayout.cshtml";
				}
			}
			return null;
		}

        public async Task<ApplicationUser> RegisterAdminService(ApplicationUserViewModel applicationUserViewModel)
        {
            if (applicationUserViewModel != null)
            {
                var newInstanceOfApplicantModelAboutToBCreated = new ApplicationUser();
                {
                    newInstanceOfApplicantModelAboutToBCreated.FirstName = applicationUserViewModel.FirstName;
                    newInstanceOfApplicantModelAboutToBCreated.LastName = applicationUserViewModel.LastName;
                    newInstanceOfApplicantModelAboutToBCreated.Email = applicationUserViewModel.Email;
                    newInstanceOfApplicantModelAboutToBCreated.UserName = applicationUserViewModel.Email;
                    newInstanceOfApplicantModelAboutToBCreated.Deactivated = false;
                    newInstanceOfApplicantModelAboutToBCreated.IsAdmin = true;
                    newInstanceOfApplicantModelAboutToBCreated.DateRegistered = DateTime.Now;
                }
                var result = await _userManager.CreateAsync(newInstanceOfApplicantModelAboutToBCreated, applicationUserViewModel.Password);
                if (result.Succeeded)
                {
                    return newInstanceOfApplicantModelAboutToBCreated;

                }
                return null;
            }
            return null;

        }

        //APPLICATION ACCEPT POST SERVICE
        public ApplicationUser AcceptSelectedApplication(ApplicationUser applicantIdToAccept)
        {
            if (applicantIdToAccept != null)
            {
                var applicantsDetails = _userManager.FindByIdAsync(applicantIdToAccept.Id).Result;

                applicantsDetails.Status = ApplicationStatus.Accepted;

                _context.ApplicationUser.Update(applicantsDetails);
                _context.SaveChanges();

                return applicantsDetails;
            }
            else
            {
                return null;
            }
        }

        //APPLICATION REJECT POST SERVICE
        public ApplicationUser RejectSelectedApplication(ApplicationUser applicantIdToReject)
        {
            if (applicantIdToReject != null)
            {
                var applicantsDetails = _userManager.FindByIdAsync(applicantIdToReject.Id).Result;

                applicantsDetails.Status = ApplicationStatus.Rejected;

                _context.ApplicationUser.Update(applicantsDetails);
                _context.SaveChanges();

                return applicantsDetails;
            }
            else
            {
                return null;
            }
        }

    }
}
