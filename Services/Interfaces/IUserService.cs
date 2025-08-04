using DTOs;
using Models.Entities;

namespace Services.Interfaces
{
    public interface IUserService
    {
        //UserDTO GetUserProfile(int userId);
        User RegisterUser(RegisterUserDTO dto);
        List<User> GetAllUsers();
        User? GetUserById(int id);
        void UpdateUser(User user);
        void DeleteUser(int id);
        //User RegisterUser(RegisterUserDTO dto);
        //void UpdateUserProfile(int userId, UpdateUserDTO dto);
        //void ChangePassword(int userId, ChangePasswordDTO dto);
        //bool IsUsernameAvailable(string username);
    }
}
