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
        public string Status { get; set; } = null!;

        // Navegación
        public virtual User User { get; set; } = null!;
        public virtual Commission Commission { get; set; } = null!;
        public virtual Grade? Grade { get; set; }
    }
}
