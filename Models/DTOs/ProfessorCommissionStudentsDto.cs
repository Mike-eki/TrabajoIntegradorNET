using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class ProfessorCommissionStudentsDto
    {
        public int CommissionId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public int CycleYear { get; set; }
        public string DayOfWeek { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public List<StudentInCommissionDto> Students { get; set; } = new();
    }
}
