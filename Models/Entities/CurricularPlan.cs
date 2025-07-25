using System.ComponentModel.DataAnnotations;

namespace Models.Entities
{
    public class CurricularPlan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string PlanName { get; set; } = null!;

        [Required]
        public int CourseId { get; set; }

        [Required]
        [MaxLength(50)]
        public string AcademicPeriod { get; set; } = null!;

        // Navegación
        public virtual Course Course { get; set; } = null!;
    }
}
