using Models.Entities;
using Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class LoginDTO
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class RegisterUserDTO
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public RoleType RoleName { get; set; }
    }

    public class ChangePasswordDTO
    {
        public string CurrentPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }

    public class UpdateUserDTO
    {
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string? Email { get; set; }

        public string? Name { get; set; }
    }

    // To show user information (not password or RoleId)
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public RoleType RoleName { get; set; }
        public List<SpecialtyDTO> Specialties { get; set; } = new List<SpecialtyDTO>(); // Specialties of the user
    }
}
