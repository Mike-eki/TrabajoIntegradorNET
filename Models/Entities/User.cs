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

        // Propiedades de navegación
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<UserSpecialty> UserSpecialties { get; set; } = new List<UserSpecialty>();
        public ICollection<Commission> Commissions { get; set; } = new List<Commission>(); // Si es Profesor
    }
}
