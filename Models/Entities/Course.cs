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


        public ICollection<Specialty> SpecialtiesLinked { get; set; } = new List<Specialty>();
    }
}
