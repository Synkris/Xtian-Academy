﻿using Core.Enum;
using Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.ViewModels
{
	public class PaymentsViewModel
	{
        public int Id { get; set; }
        public TransactionType Source { get; set; }
        public string InvoiceNumber { get; set; }
        public string ProveOfPayment { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser Student { get; set; }
        public int? CourseId { get; set; }
        [ForeignKey("CourseId")]
        public virtual TrainingCourse Courses { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime Date { get; set; }
    }
}
