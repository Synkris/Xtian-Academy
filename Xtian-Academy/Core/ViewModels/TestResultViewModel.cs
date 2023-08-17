using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.ViewModels
{
    public class TestResultViewModel
    {
        public Guid Id { get; set; }
        public int? UserId { get; set; }
        public int? CourseId { get; set; }
        public int ResultOne { get; set; }
        public bool TestOneChecker { get; set; }
        public int ResultTwo { get; set; }
        public bool TestTwoChecker { get; set; }
        public int Total { get; set; }
        public virtual TrainingCourse Courses { get; set; }
        public virtual TestResult Result { get; set; }
    }
}
