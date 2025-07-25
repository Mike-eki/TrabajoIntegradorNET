using System.ComponentModel.DataAnnotations;

namespace Models.Entities
{
    public class Grade
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EnrollmentId { get; set; }

        [Required]
        [Range(0, 10)]
        public decimal Mark { get; set; }

        [Required]
        public DateTime Date { get; set; }

        // Navegación
        public virtual Enrollment Enrollment { get; set; } = null!;
    }
}
