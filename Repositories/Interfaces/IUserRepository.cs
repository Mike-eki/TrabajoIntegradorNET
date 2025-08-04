using Models.Entities;

namespace Repositories.Interfaces
{
    public interface IUserRepository
    {
        User? GetUserByUsername(string username);
        User? GetUserById(int id);
        List<User> GetAllUsers();
        void AddUser(User user);
        void DeleteUser(int id);
        void UpdateUser(User user);
    }
}
