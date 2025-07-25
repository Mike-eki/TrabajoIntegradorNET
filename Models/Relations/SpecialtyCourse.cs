using Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Relations
{
    public class SpecialtyCourse
    {
        [Key]
        [Column(Order = 1)]
        public int SpecialtyId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int CourseId { get; set; }

        // Navegación
        public virtual Specialty Specialty { get; set; } = null!;
        public virtual Course Course { get; set; } = null!;
    }
}
