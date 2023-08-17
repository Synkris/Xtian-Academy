using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Models
{
    public class InterviewAnswerOptions
    {
        public int Id { get; set; }
        public int? QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public virtual InterviewQuestions Question { get; set; }
        public string Option { get; set; }
    }
}
