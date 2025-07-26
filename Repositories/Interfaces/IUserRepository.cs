using Models.Entities;

namespace Repositories.Interfaces
{
    public interface IUserRepository
    {
        User? GetUserByUsername(string username);
        User? GetUserById(int id);
        List<Role> GetUserRoles();

        void AddUser(User user);
    }
}
