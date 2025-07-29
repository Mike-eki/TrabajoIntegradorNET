using Models.Relations;
using Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Models.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Password { get; set; } = null!;

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        public RoleType RoleName { get; set; }

        public virtual ICollection<Specialty> Specialties { get; set; } = new List<Specialty>();
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        // Navegación

        //public virtual ICollection<StudentSpecialty> StudentSpecialties { get; set; } = new List<StudentSpecialty>();
        //public virtual ICollection<ProfessorSpecialty> ProfessorSpecialties { get; set; } = new List<ProfessorSpecialty>();
    }
}
