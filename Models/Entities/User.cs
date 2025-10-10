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
        public string PasswordHash { get; set; } = null!;

        public string Salt { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; } = UserRole.Student; // Enum con default Student

        //[Required]
        //[EmailAddress]
        //[MaxLength(100)]
        //public string Email { get; set; } = null!;

        //[Required]
        //[MaxLength(100)]
        //public string Name { get; set; } = null!;



        // Propiedades de navegación
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<UserSpecialty> UserSpecialties { get; set; } = new List<UserSpecialty>();
        public ICollection<Commission> Commissions { get; set; } = new List<Commission>(); // Si es Profesor
    }
}
