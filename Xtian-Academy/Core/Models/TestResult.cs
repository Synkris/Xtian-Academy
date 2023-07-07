using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Models
{
    public class TestResult
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser Student { get; set; }
        public int? CourseId { get; set; }
        [ForeignKey("CourseId")]
        public virtual TrainingCourse Course { get; set; }
        public int ResultOne { get; set; }
        public bool TestOneChecker { get; set; }
        public int ResultTwo { get; set; }
        public bool TestTwoChecker { get; set; }
        public int Total { get; set; }
        [NotMapped]
        public string Title { get; set; }
    }
}
