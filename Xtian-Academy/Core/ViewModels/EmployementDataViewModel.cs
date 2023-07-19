using Core.Enum;
using Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class EmployementDataViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyState { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyHrPhoneNO { get; set; }
        public DateTime MonthlyPaymentDate { get; set; }
        public string JobTitle { get; set; }
        public string OfferLetter { get; set; }
        public string Salary { get; set; }
        public string MonthlyDeduction { get; set; }
        public bool IsApproved { get; set; }
        public DateTime DateCreated { get; set; }
        public GeneralAction Action { get; set; }
    }
}
