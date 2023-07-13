using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Models
{
	public class ReoccuringPayments
	{
        [Key]
        public int MainId { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public string name { get; set; }
        public string interval { get; set; }
        public int invoice_limit { get; set; }
        public int amount { get; set; }
        public int integration { get; set; }
        public string domain { get; set; }
        public string currency { get; set; }
        public string plan_code { get; set; }
        public bool send_invoices { get; set; }
        public bool send_sms { get; set; }
        public bool hosted_page { get; set; }
        public bool migrate { get; set; }
        public bool is_archived { get; set; }
        public int id { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }

        public string authorization_url { get; set; }
        public bool IsAuthorized { get; set; }
        public string access_code { get; set; }
        public string reference { get; set; }
        public DateTime dateUpdated { get; set; }
    }
}
