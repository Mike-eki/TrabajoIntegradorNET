using Models.Entities;
using Data;
using Repositories.Interfaces;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        public User? GetUserByUsername(string username)
        {
            return InMemory.usersSample
                .FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public User? GetUserById(int id)
        {
            return InMemory.usersSample.FirstOrDefault(u => u.Id == id);
        }
    }
}
