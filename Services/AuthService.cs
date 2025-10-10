using Services.Interfaces;
using Models.Entities;
using Repositories;
using Repositories.Interfaces;

namespace Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User? Authenticate(string username, string password)
        {
            var user = _userRepository.GetUserByUsername(username);
            if (user == null) return null;

            return password == user.PasswordHash ? user : null; // Return null if password does not match
        }
    }
}
