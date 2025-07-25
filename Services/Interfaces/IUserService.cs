using DTOs;
using Models.Entities;

namespace Services.Interfaces
{
    public interface IUserService
    {
        UserDTO GetUserProfile(int userId);
        //User RegisterUser(RegisterUserDTO dto);
        //void UpdateUserProfile(int userId, UpdateUserDTO dto);
        void ChangePassword(int userId, ChangePasswordDTO dto);
        //bool IsUsernameAvailable(string username);
    }
}
