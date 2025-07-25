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
        private readonly ISpecialtyRepository _specialtyRepository;
        private readonly IRoleRepository _roleRepository;

        public UserService(IUserRepository userRepository, ISpecialtyRepository specialtyRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _specialtyRepository = specialtyRepository;
            _roleRepository = roleRepository;
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
