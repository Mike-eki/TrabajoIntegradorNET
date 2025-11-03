using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class UnassignedCommissionDto
    {
        public int Id { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public int CycleYear { get; set; }
        public string DayOfWeek { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public int EnrolledCount { get; set; }
        public int AvailableSeats { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool CanAssign => Status == "Pendiente" || string.IsNullOrEmpty(Status); // Lógica de negocio
    }
}
