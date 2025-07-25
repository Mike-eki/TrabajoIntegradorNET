using DTOs;
using Services.Interfaces;
using Models.Entities;
using Repositories.Interfaces;
using Repositories;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserDTO GetUserProfile(int userId)
        {
            var user = _userRepository.GetUserById(userId);
            if (user == null) throw new KeyNotFoundException("Usuario no encontrado");

            // Obtener especialidades del usuario
            var specialties = DataInMemory.UserSpecialties
                .Where(us => us.UserId == userId)
                .Select(us => DataInMemory.Specialties.First(s => s.Id == us.SpecialtyId))
                .ToList();

            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Name = user.Name,
                Role = DataInMemory.Roles.First(r => r.Id == user.RoleId).Name,
                Specialties = specialties.Select(s => s.Name).ToList()
            };
        }
    }
}
