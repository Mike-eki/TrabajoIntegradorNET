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
                .FirstOrDefault(u => u.Username.Equals(username, StringComparison.Ordinal));
        }

        public User? GetUserById(int id)
        {
            return InMemory.usersSample.FirstOrDefault(u => u.Id == id);
        }

        public void AddUser(User user)
        {

            if (InMemory.usersSample.Any(u => u.Username.Equals(user.Username, StringComparison.Ordinal)))
            {
                throw new InvalidOperationException("El nombre de usuario ya existe.");
            }
            if (InMemory.usersSample.Any(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("El email de usuario ya existe.");
            }

            user.Id = InMemory.usersSample.Max(u => u.Id) + 1; // Assign a new ID
            InMemory.usersSample.Add(user);

        }
    }
}
