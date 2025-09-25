using Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace Models.Relations
{
    public class UserSpecialty
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int SpecialtyId { get; set; }

        [Required]
        public DateTime EnrollDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = null!;

        // Propiedades de navegación
        public User User { get; set; } = null!; // Navegación a User
        public Specialty Specialty { get; set; } = null!; // Navegación a Specialty
    }
}
