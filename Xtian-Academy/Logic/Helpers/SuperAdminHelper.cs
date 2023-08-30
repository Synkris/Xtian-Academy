using Core.Db;
using Core.Models;
using Logic.IHelpers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Helpers
{
    public class SuperAdminHelper : ISuperAdminHelper
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SuperAdminHelper(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<ApplicationUser> GetListOfAllAdmin()
        {
            var allAdmin = _userManager.Users.Where(b => b.Status == 0).ToList();
            if (allAdmin.Any())
            {
                return allAdmin;
            }
            return allAdmin;
        }

        public ApplicationUser DeactivateAdminAccount(ApplicationUser collectedData)
        {
            var adminToDisable = _userManager.Users.Where(c => c.Id == collectedData.Id).FirstOrDefault();
            if (adminToDisable != null)
            {
                adminToDisable.Deactivated = true;

                _context.Users.Update(adminToDisable);
                _context.SaveChanges();

                return adminToDisable;
            }
            return null;
        }

        public ApplicationUser ActivateAdminAccount(ApplicationUser collectedData)
        {
            var adminToActivate = _userManager.Users.Where(c => c.Id == collectedData.Id).FirstOrDefault();
            if (adminToActivate != null)
            {
                adminToActivate.Deactivated = false;

                _context.Users.Update(adminToActivate);
                _context.SaveChanges();

                return adminToActivate;
            }
            return null;
        }
    }
}
