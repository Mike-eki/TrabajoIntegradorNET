using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework.Repositories
{
    public interface ISubjectRepository
    {
        Task<IEnumerable<Subject>> GetAllAsync();
        Task<Subject?> GetByIdAsync(int id);
        Task<Subject> AddAsync(Subject subject, int[] careerIds);
        Task UpdateAsync(Subject subject, int[] careerIds);
        Task DeleteAsync(int id);
    }
}
