using Core.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Models
{
	public class ExamDuration
    {
		[Key]
		public int Id { get; set; }
		public ExamType Type { get; set; }
		public int Duration { get; set; }
	}
}
