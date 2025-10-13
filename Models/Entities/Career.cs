using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Career
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        // Propiedad de navegación: una carrera tiene muchas materias
        public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
    }
}
