using Core.Enum;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.ViewModels
{
    public class JobViewModels
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Title { get; set; }
        public decimal Salary { get; set; }
        public JobType Type { get; set; }
        public string Description { get; set; }
        public string Responsibilities { get; set; }
        public string Requirements { get; set; }
        public bool IsActive { get; set; }
        public bool Applied { get; set; }
        public string DateCreated { get; set; }
        public GeneralAction ActionType { get; set; }
        public bool Message { get; set; }
        public bool IsProjectCompleted { get; set; }
        public List<string> DescriptionList { get; set; }
        public virtual List<JobViewModels> JobModel { get; set; }
    }
}
