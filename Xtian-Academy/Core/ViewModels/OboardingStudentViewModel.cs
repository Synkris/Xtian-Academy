using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.ViewModels
{
    public class OboardingStudentViewModel
    {
        public virtual List<ApplicationUser> ApplicantsList { get; set; }
        public virtual ApplicationUser Applicants { get; set; }
    }
}
