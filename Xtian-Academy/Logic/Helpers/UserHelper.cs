using Core.Database;
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
        //public async Task<ApplicationUser> FindByIdAsync(string Id)
        //{
        //    return await _userManager.Users.Where(s => s.Id == Id)?.FirstOrDefaultAsync();
        //}
    }
}
