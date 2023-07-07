using Core.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Models
{
    public class ApplicantDocumment
    {
        [Key]
        public Guid Id { get; set; }
        public string StudentId { get; set; }
        [ForeignKey("UserId")]
        [Display(Name = "User Id")]
        public virtual ApplicationUser StudentRequestingDetails{ get; set; }
        [Required]
        public string BVN { get; set; }
        [Required]
        [Display(Name ="First Guarantor")]
        public string FirstGuarantor { get; set; }
        [Required]
        [Display(Name = "Second Guarantor")]
        public string SecondGuarantor { get; set; }
        [Required]
        [Display(Name = "Nepa Bill")]
        public string NepaBill { get; set; }
        [Required]
        [Display(Name = "Signed Contract")]
        public string SignedContract { get; set; }
        public bool IsDeleted { get; set; }
    }
}
