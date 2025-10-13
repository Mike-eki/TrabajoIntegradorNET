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
    }

}

