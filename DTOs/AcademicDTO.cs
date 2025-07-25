using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    // Para mostrar especialidades con materias
    public class SpecialtyDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<CourseDTO> Courses { get; set; } = new List<CourseDTO>();
    }

    // Para mostrar materias con plan curricular
    public class CourseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Credits { get; set; }
        public string AcademicPeriod { get; set; } = null!; // Ej: "Primer Cuatrimestre"
    }

    // Para mostrar comisiones con horario y profesor
    public class CommissionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Shift { get; set; } = null!;
        public string Schedule { get; set; } = null!; // Ej: "Lunes 8-10 AM"
        public string? ProfessorName { get; set; } // Nombre del profesor (si está asignado)
        public int StudentLimit { get; set; }
        public int CurrentEnrollments { get; set; } // Estudiantes inscritos
        public string CourseName { get; set; } = null!;
    }
}
