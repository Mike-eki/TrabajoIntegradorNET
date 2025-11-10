using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class EnrollmentDetailDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentLegajo { get; set; } = string.Empty;
        public string StudentFullName { get; set; } = string.Empty;
        public int CommissionId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public int CycleYear { get; set; }
        public string Status { get; set; } = "ENROLLED";
        public int? FinalGrade { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public DateTime? UnenrollmentDate { get; set; }
    }
}
