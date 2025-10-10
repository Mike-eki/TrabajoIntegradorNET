using Models.Entities;

namespace Repositories.Interfaces
{
    public interface IUserRepository
    {
        User? GetUserByUsername(string username);
        List<User> GetAllUsers();

        User GetUserById(int id);
        void AddUser(User user);
        void DeleteUser(int id);
        void UpdateUser(User user);
    }
}
