using System;
using System.Collections.Generic;
using System.Text;

namespace Core.ViewModels
{
    public class AdminViewModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool Deactivated { get; set; }
        public bool Admin { get; set; }

    }
}
