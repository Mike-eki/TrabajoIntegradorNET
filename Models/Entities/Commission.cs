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
        [MaxLength(50)]
        public string Shift { get; set; } = null!;

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
        [Range(1, 1000)]
        public int StudentLimit { get; set; }

        // Navegación
        public virtual User? Professor { get; set; }
        public virtual Course Course { get; set; } = null!;
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
