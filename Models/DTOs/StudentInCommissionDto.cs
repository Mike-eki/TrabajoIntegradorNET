using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class StudentInCommissionDto
    {
        public int EnrollmentId { get; set; } // ← Clave para asignar nota
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string EnrollmentNumber { get; set; } = string.Empty;
        public int? FinalGrade { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
