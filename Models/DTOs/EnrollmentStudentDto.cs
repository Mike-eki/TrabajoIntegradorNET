using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class EnrollmentStudentDto
    {
        public int EnrollmentId { get; set; }
        public string SubjectName { get; set; } = null!;
        public string CommissionDay { get; set; } = null!;
        public string StartTime { get; set; } = null!;
        public string EndTime { get; set; } = null!;
        public int CycleYear { get; set; }
        public string Status { get; set; } = null!;
        public int? FinalGrade { get; set; }

    }
}
