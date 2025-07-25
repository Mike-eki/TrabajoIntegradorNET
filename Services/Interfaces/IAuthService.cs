using Models.Entities;

namespace Services.Interfaces
{
    public interface IAuthService
    {
        User? Authenticate(string username, string password);
    }
}
