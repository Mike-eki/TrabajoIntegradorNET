using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Models.Commissions.razor
{
    public class CommissionWithProfessorDto
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public int? ProfessorId { get; set; }
        public string? ProfessorName { get; set; }
        public int CycleYear { get; set; }
        public string DayOfWeek { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public int? AvailableAmount { get; set; }
        public string? Status { get; set; }
    }


}

