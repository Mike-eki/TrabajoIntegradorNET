using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class EnrollmentResponseDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CommissionId { get; set; }
        public string SubjectName { get; set; } = null!;
        public DateTime EnrollmentDate { get; set; }
        public int? FinalGrade { get; set; }
        public string Status { get; set; } = null!;

    }
}
