using Core.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.ViewModels
{
    public class InterviewQuestionsViewModel
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public List<string> OptionList { get; set; }
        public InterviewAnsweredQuestionss[] InterviewAnsweredQuestions { get; set; }
        public string Answer { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public GeneralAction ActionType { get; set; }
    }

    public class InterviewAnsweredQuestionss
    {
        public int questionId { get; set; }
        public string selectedAnswer { get; set; }
    }
}
