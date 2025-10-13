using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace EntityFramework.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly AppDbContext _context;

        public EnrollmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Enrollment>> GetAllAsync()
        {
            return await _context.Enrollments
                .Include(e => e.Commission)
                .ThenInclude(c => c.Subject)
                .ToListAsync();
        }

        public async Task<Enrollment?> GetByIdAsync(int id)
        {
            return await _context.Enrollments
                .Include(e => e.Commission)
                .ThenInclude(c => c.Subject)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task AddAsync(Enrollment enrollment)
        {
            // ✅ Validación 1: Commission debe existir
            var commission = await _context.Commissions.FindAsync(enrollment.CommissionId);
            if (commission == null)
                throw new InvalidOperationException("Commission not found. Create a commission before enrolling a student.");

            // ✅ Validación 2: StudentId debe existir
            // En este caso, el User (Student) vive en otra capa (ADO.NET),
            // así que no podemos validar con EF directamente.
            // Podríamos hacer validación "externa" si tuvieras un IUserRepository.
            // Por ahora, asumimos que el ID llega validado desde la capa superior.

            // ✅ Validación 3: Evitar inscribir al mismo estudiante 2 veces en la misma comisión
            bool alreadyEnrolled = await _context.Enrollments
                .AnyAsync(e => e.StudentId == enrollment.StudentId && e.CommissionId == enrollment.CommissionId);
            if (alreadyEnrolled)
                throw new InvalidOperationException("Student is already enrolled in this commission.");

            await _context.Enrollments.AddAsync(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Enrollment enrollment)
        {
            _context.Enrollments.Update(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _context.Enrollments.FindAsync(id);
            if (e != null)
            {
                _context.Enrollments.Remove(e);
                await _context.SaveChangesAsync();
            }
        }
    }
}
