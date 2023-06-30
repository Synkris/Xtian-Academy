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
    }
}
