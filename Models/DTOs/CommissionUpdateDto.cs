using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class CommissionUpdateDto
    {
        public int SubjectId { get; set; }
        public int? ProfessorId { get; set; }
        public int CycleYear { get; set; }
        public string DayOfWeek { get; set; } = null!;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; } = null!;
    }
}
