using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        // Propiedad de navegación: una materia puede estar en muchas carreras
        public ICollection<Career> Careers { get; set; } = new List<Career>();

        // Propiedad de navegación: una materia tiene muchas comisiones
        public ICollection<Commission> Commissions { get; set; } = new List<Commission>();
    }
}
