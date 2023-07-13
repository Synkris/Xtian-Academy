using Core.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Models
{
	public class SalaryRetures
	{
        [Key]
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public int ReoccuringPaymentsId { get; set; }
        [ForeignKey("ReoccuringPaymentsId")]
        public virtual ReoccuringPayments ReoccuringPayments { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
