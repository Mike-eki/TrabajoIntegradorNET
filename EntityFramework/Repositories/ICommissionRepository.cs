using Models.DTOs;
using Models.Entities;

namespace EntityFramework.Repositories
{
    public interface ICommissionRepository
    {
        Task<IEnumerable<CommissionWithProfessorDto>> GetAllCommissionsWithProfessorsAsync(CancellationToken ct = default);
        Task AssignProfessorAsync(int commissionId, int? professorId, CancellationToken ct = default);
        Task<IEnumerable<Commission>> GetAllAsync();
        Task<Commission?> GetByIdAsync(int id);
        Task AddAsync(Commission commission, CancellationToken ct);
        Task UpdateAsync(Commission commission);
        Task DeleteAsync(int id);
    }
}
