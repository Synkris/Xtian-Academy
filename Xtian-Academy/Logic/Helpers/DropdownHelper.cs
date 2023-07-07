using Core.Database;
using Core.Enum;
using Core.Models;
using Logic.IHelpers;

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
    }
}
