using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class ProfessorCommissionDto
    {
        public int CommissionId { get; set; }
        public string SubjectName { get; set; } = null!;
        public int CycleYear { get; set; }
        public string DayOfWeek { get; set; } = null!;
        public string StartTime { get; set; } = null!;
        public string EndTime { get; set; } = null!;
        public int Capacity { get; set; }
        public IEnumerable<StudentEnrollmentDto> Students { get; set; } = new List<StudentEnrollmentDto>();
    }
}
