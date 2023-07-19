using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.ViewModels
{
    public class GeneralViewModel
    {
        public virtual TrainingCourse Courses { get; set; }
        public virtual List<TestQuestions> Questions { get; set; }
        public virtual List<TestQuestionsViewModel> QuestionMain { get; set; }
        public virtual TestResult TestResults { get; set; }
    }
}
