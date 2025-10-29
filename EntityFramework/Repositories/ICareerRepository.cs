using Models.DTOs;
using Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityFramework.Repositories
{
    public interface ICareerRepository
    {
        Task<IEnumerable<Career>> GetAllAsync();
        Task<Career?> GetByIdAsync(int id);
        Task<Career> AssignSubjectsToCareer(Career career, int[] subjectIds);
        Task AddAsync(Career career);
        Task UpdateAsync(Career career);
        Task DeleteAsync(int id);
    }
}