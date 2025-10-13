using Models.Entities;

namespace EntityFramework.Repositories
{
    public interface ICommissionRepository
    {
        Task<IEnumerable<Commission>> GetAllAsync();
        Task<Commission?> GetByIdAsync(int id);
        Task AddAsync(Commission commission);
        Task UpdateAsync(Commission commission);
        Task DeleteAsync(int id);
    }
}
