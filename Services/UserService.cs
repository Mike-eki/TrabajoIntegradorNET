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
         
        public string GetUserRoleName(int roleId)
        {
            var roles = _userRepository.GetUserRoles();

            var role = roles.Find(r => r.Id == roleId);
            if (role == null)
            {
                throw new KeyNotFoundException("Rol no encontrado");
            }

            return role.Name;
        }

        public void RegisterUser(RegisterUserDTO dto)
        {
            // Aquí se implementaría la lógica para registrar un nuevo usuario
            // Validar el DTO, crear una nueva entidad User y guardarla en la base de datos
            // Por ejemplo:
            var user = new User
            {
                Username = dto.Username,
                Password = dto.Password, // Asegúrate de hashear la contraseña antes de guardarla
                Email = dto.Email,
                Name = dto.Name,
                RoleId = dto.RoleId
            };
            //
            _userRepository.AddUser(user);
        }

        //public UserDTO GetUserProfile(int userId)
        //{
        //    var user = _userRepository.GetUserById(userId);
        //    if (user == null) throw new KeyNotFoundException("Usuario no encontrado");

        //    var specialties = _specialtyRepository.GetSpecialties();
        //    var roles = _roleRepository.GetRoles();

        //    Role roleUser = roles.FirstOrDefault(r => r.Id == user.RoleId);

        //    if (roleUser == "Student")
        //    {
        //        var studentSpecialties = user.StudentSpecialties.Select(ss => ss.SpecialtyId).ToList();
        //        //return MapUserToDTO(user, specialties.Where(s => studentSpecialties.Contains(s.Id)).ToList());
        //    }
        //    else if (roleUser == "Professor")
        //    {
        //        var professorSpecialties = user.ProfessorSpecialties.Select(ps => ps.SpecialtyId).ToList();
        //        return MapUserToDTO(user, specialties.Where(s => professorSpecialties.Contains(s.Id)).ToList());
        //    }
        //    else
        //    {
        //        // For other roles, return an empty specialties list
        //        return MapUserToDTO(user, new List<Specialty>());
        //    }

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
