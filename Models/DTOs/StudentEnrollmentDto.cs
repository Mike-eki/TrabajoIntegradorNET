using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class StudentEnrollmentDto
    {
        public int EnrollmentId { get; set; }
        public int StudentId { get; set; }
        public string? StudentName { get; set; }  // opcional, si tenés el nombre del usuario
        public string Status { get; set; } = null!;
        public int? FinalGrade { get; set; }
    }
}
