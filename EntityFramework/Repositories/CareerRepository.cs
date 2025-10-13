using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework
{
    using EntityFramework.Repositories;
    using global::ADO.NET;
    using Microsoft.EntityFrameworkCore;
    using Models.Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    namespace ADO.NET
    {
        public class CareerRepository : ICareerRepository
        {
            private readonly AppDbContext _context;

            public CareerRepository(AppDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<Career>> GetAllAsync()
            {
                return await _context.Careers.ToListAsync();
            }

            public async Task<Career?> GetByIdAsync(int id)
            {
                return await _context.Careers.FindAsync(id);
            }

            public async Task AddAsync(Career career)
            {
                await _context.Careers.AddAsync(career);
                await _context.SaveChangesAsync();
            }

            public async Task UpdateAsync(Career career)
            {
                _context.Careers.Update(career);
                await _context.SaveChangesAsync();
            }

            public async Task DeleteAsync(int id)
            {
                var career = await GetByIdAsync(id);
                if (career != null)
                {
                    _context.Careers.Remove(career);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
