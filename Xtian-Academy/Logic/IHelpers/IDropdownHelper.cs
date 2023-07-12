﻿using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.IHelpers
{
    public interface IDropdownHelper
    {
        List<DropDown> GetDropDownEnumsList();
        List<TrainingCourse> DropdownOfCourses();
        List<DropDown> DropdownOfCoursesWhereIsTested(string userName);
    }
}
