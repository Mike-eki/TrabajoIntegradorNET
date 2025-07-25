using Models.Entities;

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
        public int RoleId { get; set; } // Only for register (not expose public response)
    }

    public class ChangePasswordDTO
    {
        public string CurrentPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }

    // To show user information (not password or RoleId)
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string RoleName { get; set; } = null!; // Ex: "Student", "Proffesor"
        public List<SpecialtyDTO> Specialties { get; set; } = new List<SpecialtyDTO>(); // Specialties of the user
    }
}
