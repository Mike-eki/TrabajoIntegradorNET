using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly AppDbContext _context;
        public SubjectRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Subject>> GetAllAsync()
        {
            return await _context.Subjects
                .Include(s => s.Careers)
                .ToListAsync();
        }

        public async Task<Subject?> GetByIdAsync(int id)
        {
            return await _context.Subjects
                .Include(s => s.Careers)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Subject> AddAsync(Subject subject, int[] careerIds)
        {
            // Validación: que existan todas las careers
            var careers = await _context.Careers.Where(c => careerIds.Contains(c.Id)).ToListAsync();
            if (careers.Count != careerIds.Length)
                throw new InvalidOperationException("One or more careers not found.");

            // Asignar relaciones
            foreach (var c in careers)
                subject.Careers.Add(c);

            await _context.Subjects.AddAsync(subject);
            await _context.SaveChangesAsync();
            return subject;
        }

        public async Task UpdateAsync(Subject subject, int[] careerIds)
        {
            // Obtener la entidad actual
            var existing = await _context.Subjects
                .Include(s => s.Careers)
                .FirstOrDefaultAsync(s => s.Id == subject.Id);

            if (existing == null) throw new KeyNotFoundException();

            existing.Name = subject.Name;

            // Validar careers
            var careers = await _context.Careers.Where(c => careerIds.Contains(c.Id)).ToListAsync();
            if (careers.Count != careerIds.Length)
                throw new InvalidOperationException("One or more careers not found.");

            // Reemplazar coleccion de careers (manejo de many-to-many)
            existing.Careers.Clear();
            foreach (var c in careers) existing.Careers.Add(c);

            _context.Subjects.Update(existing);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var s = await _context.Subjects.FindAsync(id);
            if (s != null)
            {
                _context.Subjects.Remove(s);
                await _context.SaveChangesAsync();
            }
        }
    }
}
