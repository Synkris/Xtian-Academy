using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum
{
    public enum YesNoEnum
    {
        [Description("Yes")]
        Yes = 1,
        [Description("No")]
        No = 2,
    }
}
