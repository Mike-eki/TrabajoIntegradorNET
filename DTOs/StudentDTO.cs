using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    // Para inscribirse a una especialidad
    public class EnrollInSpecialtyDTO
    {
        public int SpecialtyId { get; set; }
    }

    // Para inscribirse a una comisión
    public class EnrollInCommissionDTO
    {
        public int CommissionId { get; set; }
    }

    // Para mostrar materias inscritas y sus notas
    public class EnrolledCourseDTO
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = null!;
        public string SpecialtyName { get; set; } = null!;
        public decimal? Grade { get; set; } // Nota final (si existe)
        public string AcademicPeriod { get; set; } = null!; // Cuatrimestre/semestre
    }
}
