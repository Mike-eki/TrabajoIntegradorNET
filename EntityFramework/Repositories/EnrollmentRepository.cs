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

        public async Task<IEnumerable<Enrollment>> GetAllAsync(bool includeWithdrawn = false)
        {
            var query = _context.Enrollments
                .Include(e => e.Commission)
                .ThenInclude(c => c.Subject)
                .AsQueryable();

            if (!includeWithdrawn)
                query = query.Where(e => e.Status != "WITHDRAWN");

            return await query.ToListAsync();
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
            var commission = await _context.Commissions
                .Include(c => c.Enrollments)
                .FirstOrDefaultAsync(c => c.Id == enrollment.CommissionId);

            if (commission == null)
                throw new InvalidOperationException("Commission not found. Create a commission before enrolling a student.");

            // ✅ Validación 2: Evitar inscripciones duplicadas activas
            // ✅ Verificar si existe una inscripción anterior (activa o dada de baja)
            var previous = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == enrollment.StudentId
                                       && e.CommissionId == enrollment.CommissionId);

            if (previous != null)
            {
                if (previous.Status == "ENROLLED")
                    throw new InvalidOperationException("Student is already enrolled in this commission.");

                if (previous.Status == "WITHDRAWN")
                {
                    // ✨ Reinscripción: reactivar registro anterior
                    previous.Status = "ENROLLED";
                    previous.UnenrollmentDate = null;
                    previous.EnrollmentDate = DateTime.UtcNow;
                    _context.Enrollments.Update(previous);
                    await _context.SaveChangesAsync();
                    return;
                }
            }

                // ✅ Validación 3: Verificar capacidad
                int activeCount = commission.Enrollments.Count(e => e.Status == "ENROLLED");
            if (activeCount >= commission.Capacity)
                throw new InvalidOperationException("Commission is full. No seats available.");

            // ✅ Crear inscripción
            await _context.Enrollments.AddAsync(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Enrollment enrollment)
        {
            var existing = await _context.Enrollments.FindAsync(enrollment.Id);
            if (existing == null)
                throw new KeyNotFoundException("Enrollment not found.");

            // ✏️ Actualizamos los campos básicos
            existing.FinalGrade = enrollment.FinalGrade;
            existing.Status = enrollment.Status;

            // ✨ Si tiene nota final, actualizamos el estado automáticamente
            if (enrollment.FinalGrade.HasValue)
            {
                if (enrollment.FinalGrade.Value >= 6)
                    existing.Status = "APPROVED";
                else
                    existing.Status = "FAILED";
            }

            _context.Enrollments.Update(existing);
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

        public async Task EnrollStudentAsync(int commissionId, int studentId, CancellationToken ct = default)
        {
            var commission = await _context.Commissions.FindAsync(commissionId);
            if (commission == null)
                throw new InvalidOperationException("Commission not found.");

            // Validar capacidad
            var currentCount = await _context.Enrollments.CountAsync(e => e.CommissionId == commissionId, ct);
            if (currentCount >= commission.Capacity)
                throw new InvalidOperationException("No hay cupos disponibles en esta comisión.");

            // Crear inscripción
            var enrollment = new Enrollment
            {
                CommissionId = commissionId,
                StudentId = studentId,
                EnrollmentDate = DateTime.UtcNow
            };

            await _context.Enrollments.AddAsync(enrollment, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<Commission?> GetCommissionWithEnrollmentsAsync(int commissionId, CancellationToken ct = default)
        {
            return await _context.Commissions
                .Include(c => c.Enrollments)
                .FirstOrDefaultAsync(c => c.Id == commissionId, ct);
        }

        public async Task AddEnrollmentsAsync(IEnumerable<Enrollment> enrollments, CancellationToken ct = default)
        {
            await _context.Enrollments.AddRangeAsync(enrollments, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<int> GetEnrollmentCountAsync(int commissionId, CancellationToken ct = default)
        {
            return await _context.Enrollments
                .CountAsync(e => e.CommissionId == commissionId, ct);
        }

    }
}
