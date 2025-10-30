using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
    public class Enrollment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; } // FK hacia User (Student)

        [Required]
        [ForeignKey(nameof(Commission))]
        public int CommissionId { get; set; }
        public Commission Commission { get; set; } = null!;

        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;

        public DateTime? UnenrollmentDate { get; set; } // ✨ Nueva propiedad

        public int? FinalGrade { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Inscripto"; // Inscripto, Pendiente, Cerrado
    }
}

