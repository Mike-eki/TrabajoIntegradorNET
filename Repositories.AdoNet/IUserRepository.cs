using Models.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace ADO.NET
{
    public interface IUserRepository
    {
        Task<bool> ExistsAsync(string username, CancellationToken ct = default);
        Task CreateAsync(User user, CancellationToken ct = default);
        Task UpdateAsync(User user, CancellationToken ct = default);
        Task DeleteAsync(int userId, CancellationToken ct = default);
        Task<List<User>> GetAllAsync(CancellationToken ct = default);
        Task<User?> GetByIdAsync(int userId, CancellationToken ct = default);
        Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default);
    }
}