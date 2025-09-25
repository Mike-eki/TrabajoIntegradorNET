using Models.Relations;
using System.ComponentModel.DataAnnotations;

namespace Models.Entities
{
    public class Grade
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        [Range(0, 10)]
        public decimal Mark { get; set; }

        [Required]
        public DateTime Date { get; set; }

        // Navegación
        public virtual User User { get; set; } = null!;
        public virtual Course Course { get; set; } = null!;

    }
}
