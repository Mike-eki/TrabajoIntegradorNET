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
                return await _context.Careers
                    .Include(c => c.Subjects)
                    .ToListAsync();
            }

            public async Task<Career?> GetByIdAsync(int id)
            {
                return await _context.Careers
                    .Include(c => c.Subjects)
                    .FirstOrDefaultAsync(c => c.Id == id);
            }

            public async Task<List<int>> GetSubjectIdsForCareerAsync(int careerId, CancellationToken ct)
            {
                return await _context.Careers
                            .Where(c => c.Id == careerId)
                            .SelectMany(c => c.Subjects)
                            .Select(s => s.Id)
                            .ToListAsync(ct);
            }
            public async Task<Career> AssignSubjectsToCareer(Career career, int[] subjectIds)
            {
                // Validación: que existan todas las subjects
                var subjects = await _context.Subjects
                                            .Where(s => subjectIds.Contains(s.Id))
                                            .ToListAsync();
                if (subjects.Count != subjectIds.Length)
                    throw new InvalidOperationException("One or more careers not found.");

                // Limpiar la lista
                career.Subjects.Clear();

                // Asignar relaciones
                foreach (var s in subjects)
                {
                    career.Subjects.Add(s);
                    s.Careers.Add(career);   // <‑‑ mantener la navegación inversa
                }

                _context.Careers.Update(career);
                await _context.SaveChangesAsync();

                // Cargar la navegación inversa para que el objeto devuelto esté “completo”
                await _context.Entry(career)
                  .Collection(c => c.Subjects)
                  .LoadAsync();


                return career;
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
