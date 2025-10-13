using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
    public class Commission
    {
        [Key]
        public int Id { get; set; }

        // FK → Subject (una materia tiene muchas comisiones)
        [Required]
        [ForeignKey(nameof(Subject))]
        public int SubjectId { get; set; }
        public Subject Subject { get; set; } = null!;

        // FK → User (profesor asignado, puede ser nulo)
        public int? ProfessorId { get; set; }

        // ⚠️ No establecemos aquí la navegación hacia User
        // porque User está en el dominio ADO.NET, no en EFCore.
        // Podremos representarlo más adelante si unificamos modelos.

        [Required]
        public int CycleYear { get; set; } // Ej: 2025

        [Required]
        [MaxLength(20)]
        public string DayOfWeek { get; set; } = null!; // Ej: "Lunes"

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "INACTIVE"; // Ej: INACTIVE, ACTIVE, FINALIZED

        // Navegación inversa: una comisión puede tener muchas inscripciones
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
