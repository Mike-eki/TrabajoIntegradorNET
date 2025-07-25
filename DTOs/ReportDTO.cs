using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    // Reporte de cursos (por comisión y estudiantes)
    public class CourseReportDTO
    {
        public string CourseName { get; set; } = null!;
        public string SpecialtyName { get; set; } = null!;
        public int TotalEnrollments { get; set; } // Total de estudiantes inscritos
        public decimal AverageGrade { get; set; } // Promedio de notas
        public int Credits { get; set; }
    }

    // Reporte de planes curriculares (por especialidad)
    public class CurricularPlanReportDTO
    {
        public string SpecialtyName { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public int Credits { get; set; }
        public string AcademicPeriod { get; set; } = null!;
    }
}
