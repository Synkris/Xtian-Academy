using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Models
{
    public class JobApplication
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser Applicant { get; set; }
        public int JobId { get; set; }
        [ForeignKey("JobId")]
        public virtual Job Job { get; set; }
        public DateTime DateApplied { get; set; }
    }
}
