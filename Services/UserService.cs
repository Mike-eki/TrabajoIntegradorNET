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
         
        //public string GetUserRoleName(int roleId)
        //{
        //    var roles = _userRepository.GetUserRoles();

        //    var role = roles.Find(r => r.Id == roleId);
        //    if (role == null)
        //    {
        //        throw new KeyNotFoundException("Rol no encontrado");
        //    }

        //    return role.Name;
        //}

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
                Password = dto.Password,
                Email = dto.Email,
                Name = dto.Name,
                RoleName = dto.RoleName
            };

            _userRepository.AddUser(user);
            
            return user;
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
