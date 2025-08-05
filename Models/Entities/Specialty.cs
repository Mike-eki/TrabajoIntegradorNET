using Models.Relations;
using System.ComponentModel.DataAnnotations;

namespace Models.Entities
{
    public class Specialty
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
    }
}
