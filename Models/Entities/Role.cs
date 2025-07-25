using System.ComponentModel.DataAnnotations;

namespace Models.Entities
{ 
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        // Navegación
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}