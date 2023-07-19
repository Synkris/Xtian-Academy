using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.ViewModels
{
    public class InterviewViewModel
    {
        public bool  IsWritten { get; set; }
        public int  Duration { get; set; }
        public virtual List<InterviewQuestionsViewModel> Questions { get; set; }
    }
}
