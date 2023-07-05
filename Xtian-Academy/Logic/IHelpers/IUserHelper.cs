using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.IHelpers
{
    public interface IUserHelper
    {
        string GetFullNameByUserNameAsync(string userName);
        Task<ApplicationUser> FindByUserNameAsync(string userName);
        Task<ApplicationUser> FindByPhoneNumberAsync(string phoneNumber);
        Task<ApplicationUser> FindByEmailAsync(string email);
        //Task<ApplicationUser> FindByIdAsync(string Id);
        Task<UserVerification> CreateUserToken(string userEmail);
        Task<UserVerification> GetUserToken(Guid token);
        Task<bool> MarkTokenAsUsed(UserVerification userVerification);
    }
}
