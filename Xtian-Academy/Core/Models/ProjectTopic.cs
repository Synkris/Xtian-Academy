using Core.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Models
{
	public class ProjectTopic
	{
		public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser Student { get; set; }
        public int? CourseId { get; set; }
        [ForeignKey("CourseId")]
        public virtual TrainingCourse Course { get; set; }
        public string Title { get; set; }	
		public string Description { get; set; }	
		public bool IsApproved { get; set; }	
		public bool LinksIsAdded { get; set; }	
		public string GitLink { get; set; }	
		public string RedmineLink { get; set; }	
		public ProjectStatus Status { get; set; }	
		public DateTime DatePosted { get; set; }	

	}
}
