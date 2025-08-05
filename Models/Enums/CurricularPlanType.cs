using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Enums
{
    public enum CurricularPlanType
    {
        [Description("Basic")]
        Basic,
        [Description("Advanced")]
        Advanced,
        [Description("Specialization")]
        Specialization,
        [Description("Master")]
        Master,
        [Description("Doctorate")]
        Doctorate,
    }
}
