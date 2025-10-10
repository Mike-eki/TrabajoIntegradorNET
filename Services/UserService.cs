using Data;
using DTOs;
using Models.Entities;
using Models.Enums;
using Repositories;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User RegisterUser(RegisterUserDTO dto)
        {

            if (!Enum.IsDefined(typeof(RoleType), dto.RoleName))
            {
                var validRoles = string.Join(", ", Enum.GetNames(typeof(RoleType)));
                throw new ArgumentException($"El rol '{dto.RoleName}' no es válido. Roles permitidos: {validRoles}");
            }

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = dto.Password,
                Email = dto.Email,
                Name = dto.Name,
                RoleName = dto.RoleName
            };

            _userRepository.AddUser(user);
            
            return user;
        }

        public List<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public User? GetUserById(int id)
        {
            return _userRepository.GetUserById(id);
        }

        public void UpdateUser(User existingUser)
        {
            // Save changes (if needed, depending on your repository implementation)
            _userRepository.UpdateUser(existingUser);
        }

        public void DeleteUser(int id)
        {
            _userRepository.DeleteUser(id);
        }

        //public UserDTO GetUserProfile(int userId)
        //{
        //    var user = _userRepository.GetUserById(userId);
        //    if (user == null) throw new KeyNotFoundException("Usuario no encontrado");

        //    return new UserDTO
        //    {
        //        Id = user.Id,
        //        Username = user.Username,
        //        Email = user.Email,
        //        Name = user.Name,
        //        Role = DataInMemory.Roles.First(r => r.Id == user.RoleId).Name,
        //        Specialties = specialties.Select(s => s.Id == user.Speac).ToList()
        //    };
        //}
    }
}
