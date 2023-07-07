﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Core.ViewModels
{
    public class TrainingCourseViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public string Duration { get; set; }
        public int TestDuration { get; set; }
        public string Logo { get; set; }
        public DateTime DateCreated { get; set; }
        public string ActionType { get; set; }
        
    }
}
