using Core.Database;
using Core.Enum;
using Core.Models;
using Logic.IHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Logic.Helpers
{
    public class UserHelper: IUserHelper
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public UserHelper(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        public string GetFullNameByUserNameAsync(string userName)
        {
            var user = _userManager.Users.Where(u => u.UserName == userName).FirstOrDefaultAsync().Result;
            var fullName = user.LastName + " " + user.FirstName;
            return fullName;
        }
        public async Task<ApplicationUser> FindByUserNameAsync(string userName)
        {
            return _userManager.Users.Where(u => u.UserName == userName).FirstOrDefaultAsync().Result;
        }
        public async Task<ApplicationUser> FindByPhoneNumberAsync(string phoneNumber)
        {
            return await _userManager.Users.Where(s => s.PhoneNumber == phoneNumber)?.FirstOrDefaultAsync();
        }
        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await _userManager.Users.Where(s => s.Email == email)?.FirstOrDefaultAsync();
        }
        public async Task<ApplicationUser> FindByIdAsync(string Id)
        {
            return await _userManager.Users.Where(s => s.Id == Id)?.FirstOrDefaultAsync();
        }

        public async Task<UserVerification> CreateUserToken(string userEmail)
        {
            try
            {
                var user = await FindByEmailAsync(userEmail);
                if (user != null)
                {
                    UserVerification userVerification = new UserVerification()
                    {
                        UserId = user.Id,
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
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<UserVerification> GetUserToken(Guid token)
        {
            return await _context.UserVerifications.Where(t => !t.Used && t.Token == token)?.Include(s => s.User).FirstOrDefaultAsync();

        }
        public async Task<bool> MarkTokenAsUsed(UserVerification userVerification)
        {
            try
            {
                var VerifiedUser = _context.UserVerifications.Where(s => s.UserId == userVerification.User.Id && s.Used != true).FirstOrDefault();
                if (VerifiedUser != null)
                {
                    userVerification.Used = true;
                    userVerification.DateUsed = DateTime.Now;
                    _context.Update(userVerification);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ApplicantDocumment GetApplicationDocummentByUserId(string userID)
        {
            var myDoc = _context.ApplicantDocumments.Where(s => s.StudentId == userID).FirstOrDefault();
            return myDoc;
        }
        public List<int> GetListOfCourseIdStudentPaid4(string userID)
        {
            var getListOfCourseStudentPaidFor = _context.Payments.Where(t => t.UserId == userID && t.Status == PaymentStatus.Approved).ToList();
            var paidCourseIDs = getListOfCourseStudentPaidFor.Select(x => x.CourseId).ToList();
            if (paidCourseIDs.Any())
            {
                return paidCourseIDs;
            }
            return null;
        }
        public TrainingVideos GetVideosById(Guid Id)
        {
            var videos = _context.TrainingVideos.Where(t => t.Id == Id && t.IsActive).FirstOrDefault();
            if (videos != null)
            {
                return videos;
            }
            return videos;
        }

        public List<ApplicationUser> GetAllOnboardApplicantsFromDB()
        {
            var allApplicants = _context.ApplicationUser.Where(b => b.Deactivated == false && b.IsAdmin == false).OrderByDescending(d => d.DateRegistered).ToList();
            if (allApplicants.Any())
            {
                return allApplicants;
            }
            return allApplicants;
        }

        public List<Payments> GetPaymentList()
        {
            var allPayments = _context.Payments.Include(p => p.Student).Include(p => p.Courses).OrderByDescending(d => d.Date).ToList();
            if (allPayments.Any())
            {
                return allPayments;
            }
            return allPayments;
        }

        public Payments GetPaymentById(int? Id)
        {
            var payment = _context.Payments.Where(t => t.Id == Id).Include(c => c.Courses).Include(s => s.Student).FirstOrDefault();
            if (payment != null)
            {
                return payment;
            }
            return payment;
        }

        public List<TrainingCourse> GetAllTrainingCourseFromDB()
        {
            var allTrainingCourse = _context.TrainingCourse.Where(t => !t.IsDeleted).ToList();
            if (allTrainingCourse.Any())
            {
                return allTrainingCourse;
            }
            return allTrainingCourse;
        }

        public TrainingCourse GetTrainingCourseById(int? Id)
        {
            var allTrainingCourse = _context.TrainingCourse.Where(t => t.Id == Id).FirstOrDefault();
            if (allTrainingCourse != null)
            {
                return allTrainingCourse;
            }
            return allTrainingCourse;
        }
        public List<TrainingVideos> GetTrainingVideos()
        {
            var allVideos = _context.TrainingVideos.Where(v => v.IsActive).Include(c => c.Name).ToList();
            if (allVideos.Any())
            {
                return allVideos;
            }
            return allVideos;
        }

    }
}
