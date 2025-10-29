using Models.DTOs;

namespace Services.Interfaces
{
    public interface ICommissionService
    {
        Task<CommissionWithProfessorDto?> GetByIdAsync(int id, CancellationToken ct = default);
    }
}
