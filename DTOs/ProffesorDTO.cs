using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    // To enrole in commision (enable commision)
    public class EnrollInCommissionAsProfessorDTO
    {
        public int CommissionId { get; set; }
    }

    // To show students on one commission
    public class StudentEnrollmentDTO
    {
        public int UserId { get; set; }
        public string StudentName { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public string CommissionName { get; set; } = null!;
        public decimal? Grade { get; set; }
        public string Status { get; set; } = null!; // Active, Pending, Descesion, etc.
    }

    // To assign a final mark
    public class AssignGradeDTO
    {
        public int EnrollmentId { get; set; }
        [Range(0, 10)]
        public decimal Mark { get; set; }
        public string? Comments { get; set; }
    }
}
