using Models.Enums;
using Models.Relations;
using System.ComponentModel.DataAnnotations;

namespace Models.Entities
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        public AcademicPeriodType AcademicPeriod;

        [Required]
        public List<Specialty> SpecialtiesLinked { get; set; } = new List<Specialty>();

        [Required]
        public CurricularPlanType CurricularPlan;

        // Navegación
        //public virtual CurricularPlan CurricularPlan { get; set; } = null!;
        //public virtual ICollection<SpecialtyCourse> SpecialtyCourses { get; set; } = new List<SpecialtyCourse>();
        //public virtual ICollection<Commission> Commissions { get; set; } = new List<Commission>();
    }
}
