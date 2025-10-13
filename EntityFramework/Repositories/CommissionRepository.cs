using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace EntityFramework.Repositories
{
    public class CommissionRepository : ICommissionRepository
    {
        private readonly AppDbContext _context;

        public CommissionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Commission>> GetAllAsync()
        {
            return await _context.Commissions
                .Include(c => c.Subject)
                .ToListAsync();
        }

        public async Task<Commission?> GetByIdAsync(int id)
        {
            return await _context.Commissions
                .Include(c => c.Subject)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Commission commission)
        {
            // ✅ Validar existencia del Subject antes de insertar
            var subject = await _context.Subjects.FindAsync(commission.SubjectId);
            if (subject == null)
                throw new InvalidOperationException("Subject not found. Create the subject before creating a commission.");

            await _context.Commissions.AddAsync(commission);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Commission commission)
        {
            _context.Commissions.Update(commission);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Commissions.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
