using Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace Models.Relations
{
    public class StudentSpecialty
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

        // Navegación
        public virtual User User { get; set; } = null!;
        public virtual Specialty Specialty { get; set; } = null!;
    }
}
