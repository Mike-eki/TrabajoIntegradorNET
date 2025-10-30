using Models.DTOs;
using Models.Entities;

namespace EntityFramework.Repositories
{
    public interface IEnrollmentRepository
    {
        Task<IEnumerable<Enrollment>> GetAllAsync(bool includeWithdrawn = false);
        Task<Enrollment?> GetByIdAsync(int id);
        Task AddAsync(Enrollment enrollment);
        Task UpdateAsync(Enrollment enrollment);
        Task DeleteAsync(int id);
        Task EnrollStudentAsync(int commissionId, int studentId, CancellationToken ct = default);

        Task<Commission?> GetCommissionWithEnrollmentsAsync(int commissionId, CancellationToken ct = default);
        Task AddEnrollmentsAsync(IEnumerable<Enrollment> enrollments, CancellationToken ct = default);
        Task<int> GetEnrollmentCountAsync(int commissionId, CancellationToken ct = default);

        Task<List<Enrollment>> GetAllEnrollmentsAsync(CancellationToken ct = default);
    }

}

