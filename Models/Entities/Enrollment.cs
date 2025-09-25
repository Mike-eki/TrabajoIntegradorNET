using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Models.Entities
{
    public class Enrollment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int CommissionId { get; set; }

        [Required]
        public DateTime EnrollDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Activo";

        // Propiedades de navegación
        public User User { get; set; } = null!; // Navegación a User
        public Commission Commission { get; set; } = null!; // Navegación a Commission
        public ICollection<Grade> Grades { get; set; } = new List<Grade>(); // Navegación a Grades
    }
}
