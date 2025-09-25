using System.ComponentModel.DataAnnotations;

namespace Models.Entities
{
    public class Commission
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(10)]
        public string Day { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Schedule { get; set; } = null!;

        public int? ProfessorId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public bool Enabled { get; set; }

        [Required]
        [Range(5, 100)]
        public int MaxEnrolls { get; set; }

        // Propiedades de navegación
        public User Professor { get; set; } = null!; // Navegación a User (Profesor)
        public Course Course { get; set; } = null!; // Navegación a Course
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>(); // Navegación a Enrollments
    }
}
