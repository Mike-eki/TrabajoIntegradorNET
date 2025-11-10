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
        public string Fullname { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Legajo { get; set; } = null!;

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string PasswordHash { get; set; } = null!;

        public string Salt { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; } = UserRole.Student; // Enum con default Student


    }
}
