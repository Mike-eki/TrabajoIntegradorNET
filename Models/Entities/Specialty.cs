using Models.Relations;
using System.ComponentModel.DataAnnotations;

namespace Models.Entities
{
    public class Specialty
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        // Navegación
        //public virtual ICollection<StudentSpecialty> UserSpecialties { get; set; } = new List<StudentSpecialty>();
        //public virtual ICollection<ProfessorSpecialty> ProfessorSpecialties { get; set; } = new List<ProfessorSpecialty>();
        //public virtual ICollection<SpecialtyCourse> SpecialtyCourses { get; set; } = new List<SpecialtyCourse>();
    }
}
