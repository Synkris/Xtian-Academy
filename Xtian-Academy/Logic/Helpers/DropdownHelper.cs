﻿using Core.Database;
using Core.Enum;
using Core.Models;
using Logic.IHelpers;
using Microsoft.EntityFrameworkCore;

namespace Logic.Helpers
{
    public class DropdownHelper: IDropdownHelper
    {
        private readonly AppDbContext _context;

        public DropdownHelper(AppDbContext context)
        {
            _context = context;
        }

        public List<DropDown> GetDropDownEnumsList()
        {
            var common = new DropDown()
            {
                Id = 0,
                Name = "-- Select --"
            };
            var enumList = ((YesNoEnum[])Enum.GetValues(typeof(YesNoEnum))).Select(c => new DropDown() { Id = (int)c, Name = c.ToString() }).ToList();
            enumList.Insert(0, common);
            return enumList;
        }

        public List<TrainingCourse> DropdownOfCourses()
        {
            try
            {
                var common = new TrainingCourse()
                {
                    Id = 0,
                    Title = "Select Course"
                };
                var listOfCourses = _context.TrainingCourse.Where(x => x.Id != 0 && x.IsActive == true && x.IsDeleted == false).OrderBy(p => p.Title).ToList();
                listOfCourses.Insert(0, common);
                return listOfCourses;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
        public List<DropDown> DropdownOfCoursesWhereIsTested(string userName)
        {
            try
            {
                var common = new DropDown()
                {
                    Id = 0,
                    Name = "Select Course"
                };
                var user = _context.Users.Where(u => u.UserName == userName).FirstOrDefault();
                var listOfTestTaken = _context.TestResults.Where(x => x.UserId == user.Id).Include(x => x.Course).ToList();
                var drp = listOfTestTaken.Select(x => new DropDown
                {
                    Id = x.Course.Id,
                    Name = x.Course.Title,
                }).ToList();


                drp.Insert(0, common);
                return drp;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        public List<DropDown> JobTypesForSearch()
        {
            var newJobTypeList = new List<DropDown>();
            var common = new DropDown()
            {
                Id = 0,
                Name = "-- Select job type --"
            };
            var jobList = ((JobType[])Enum.GetValues(typeof(JobType))).Select(c => new DropDown() { Id = (int)c, Name = c.ToString() }).ToList();
            foreach (var jobType in jobList)
            {
                if (jobType.Name == "Full_Time_Premise")
                {
                    var newCommon = new DropDown();
                    newCommon.Id = jobType.Id;
                    newCommon.Name = "Full-time (On Premise)";
                    newJobTypeList.Add(newCommon);
                }
                else if (jobType.Name == "Part_Time_Premise")
                {
                    var newCommon = new DropDown();
                    newCommon.Id = jobType.Id;
                    newCommon.Name = "Part-time (On Premise)";
                    newJobTypeList.Add(newCommon);
                }
                else if (jobType.Name == "Full_Time_Home")
                {
                    var newCommon = new DropDown();
                    newCommon.Id = jobType.Id;
                    newCommon.Name = "Full-time (On Home)";
                    newJobTypeList.Add(newCommon);
                }
                else
                {
                    var newCommon = new DropDown();
                    newCommon.Id = jobType.Id;
                    newCommon.Name = "Part-time (On Home)";
                    newJobTypeList.Add(newCommon);
                }
            }
            newJobTypeList.Insert(0, common);
            return newJobTypeList;
        }

        public List<DropDown> JobTypes()
        {
            var newJobTypeList = new List<DropDown>();
            var common = new DropDown()
            {
                Id = 0,
                Name = "-- Select --"
            };
            var jobList = ((JobType[])Enum.GetValues(typeof(JobType))).Select(c => new DropDown() { Id = (int)c, Name = c.ToString() }).ToList();
            foreach (var jobType in jobList)
            {
                if (jobType.Name == "Full_Time_Premise")
                {
                    var newCommon = new DropDown();
                    newCommon.Id = jobType.Id;
                    newCommon.Name = "Full-time (On Premise)";
                    newJobTypeList.Add(newCommon);
                }
                else if (jobType.Name == "Part_Time_Premise")
                {
                    var newCommon = new DropDown();
                    newCommon.Id = jobType.Id;
                    newCommon.Name = "Part-time (On Premise)";
                    newJobTypeList.Add(newCommon);
                }
                else if (jobType.Name == "Full_Time_Home")
                {
                    var newCommon = new DropDown();
                    newCommon.Id = jobType.Id;
                    newCommon.Name = "Full-time (On Home)";
                    newJobTypeList.Add(newCommon);
                }
                else
                {
                    var newCommon = new DropDown();
                    newCommon.Id = jobType.Id;
                    newCommon.Name = "Part-time (On Home)";
                    newJobTypeList.Add(newCommon);
                }
            }
            newJobTypeList.Insert(0, common);
            return newJobTypeList;
        }

    }
}
