using Core.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Models
{
    public class Job
    {
        [Key]
        public int Id { get; set; } 
        public string CompanyName { get; set; }
        public string Title { get; set; }
        public decimal Salary{ get; set; }
        public JobType Type { get; set; }
        public string Description { get; set; }
        public string Responsibilities { get; set; }
        public string Requirements { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
