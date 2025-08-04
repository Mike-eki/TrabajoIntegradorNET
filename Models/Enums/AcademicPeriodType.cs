using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Enums
{
    public enum AcademicPeriodType
    {
        [Description("Semester")]
        Semester,
        [Description("Quarter")]
        Quarter,
        [Description("Trimester")]
        Trimester,
        [Description("Year")]
        Year
    }
}
