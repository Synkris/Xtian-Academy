using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Models
{
    public class TrainingVideos
    {
        [Key]
        public Guid Id { get; set; }
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public virtual TrainingCourse Name { get; set; }
        public string VideoLink { get; set; }
        public string Outline { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
