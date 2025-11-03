using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Entities;
using ADO.NET;
using Services;

namespace EntityFramework.Repositories
{
    public class CommissionRepository : ICommissionRepository
    {
        private readonly AppDbContext _context;
        private readonly IUserRepository _userRepository;

        public CommissionRepository(AppDbContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        public async Task<List<Commission>> GetCommissionByProfessorIdAsync(int professorId ,CancellationToken ct = default)
        {
            return await _context.Commissions
                .Where(c => c.ProfessorId == professorId)
                .ToListAsync(ct);
        }

        public async Task<List<Commission>> GetUnassignedCommissionsAsync(CancellationToken ct)
        {
            return await _context.Commissions
                .Where(c => c.ProfessorId == null)
                .ToListAsync(ct);
        }


        public async Task<IEnumerable<Commission>> GetAllAsync()
        {
            return await _context.Commissions
                .Include(c => c.Subject)
                .ToListAsync();
        }

        public async Task<IEnumerable<CommissionWithProfessorDto>> GetAllCommissionsWithProfessorsAsync(CancellationToken ct = default)
        {
            // 1️⃣ Cargar comisiones con sus subjects
            var commissions = await _context.Commissions
                .Include(c => c.Subject)
                .ToListAsync(ct);

            // 2️⃣ Recolectar todos los ProfessorIds válidos
            var professorIds = commissions
                .Where(c => c.ProfessorId.HasValue)
                .Select(c => c.ProfessorId.Value)
                .Distinct()
                .ToList();

            // 3️⃣ Obtener todos los profesores de una sola vez
            var professors = new Dictionary<int, string>();
            foreach (var id in professorIds)
            {
                var prof = await _userRepository.GetByIdAsync(id, ct);
                if (prof != null)
                    professors[id] = prof.Fullname;
            }

            // 4️⃣ Mapear DTOs
            var result = commissions.Select(c => new CommissionWithProfessorDto
            {
                Id = c.Id,
                SubjectId = c.SubjectId,
                SubjectName = c.Subject?.Name ?? "(Sin materia)",
                ProfessorId = c.ProfessorId,
                ProfessorName = c.ProfessorId.HasValue
                    ? professors.TryGetValue(c.ProfessorId.Value, out var name)
                        ? name
                        : "(Profesor no encontrado)"
                    : "(Sin profesor asignado)",
                CycleYear = c.CycleYear,
                DayOfWeek = c.DayOfWeek,
                StartTime = ScheduleHelper.FormatTimeSpan(c.StartTime),
                EndTime = ScheduleHelper.FormatTimeSpan(c.EndTime),
                Capacity = c.Capacity,
                Status = c.Status
            });

            return result.ToList();
        }

        public async Task<Commission?> GetByIdAsync(int id)
        {
            return await _context.Commissions
                .Include(c => c.Subject)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Commission commission, CancellationToken ct)
        {
            // ✅ Validar existencia del Subject antes de insertar
            var subject = await _context.Subjects.FindAsync(commission.SubjectId);
            if (subject == null)
                throw new InvalidOperationException(
                    "Subject not found. Create the subject before creating a commission.");

            // Validar Professor si se asignó
            if (commission.ProfessorId != null)
            {
                var professor = await _userRepository.GetByIdAsync(commission.ProfessorId.Value, ct);
                if (professor == null)
                    throw new InvalidOperationException("Professor not found.");
                if (professor.Role.ToString() != "Professor")
                    throw new InvalidOperationException("The user is not a Professor.");
            }

            await _context.Commissions.AddAsync(commission, ct);
            await _context.SaveChangesAsync(ct);

        }


        public async Task UpdateAsync(Commission commission)
        {
            _context.Commissions.Update(commission);
            await _context.SaveChangesAsync();
        }

        public async Task AssignProfessorAsync(int commissionId, int? professorId, CancellationToken ct = default)
        {
            var commission = await _context.Commissions.FindAsync(commissionId);
            if (commission == null)
                throw new InvalidOperationException("Commission not found.");

            // Desasignar profesor
            if (professorId == null)
            {
                commission.ProfessorId = null;
                commission.Status = "Pendiente";
                await _context.SaveChangesAsync(ct);
                return;
            }

            // Validar profesor (vía ADO.NET)
            var professor = await _userRepository.GetByIdAsync(professorId.Value, ct);
            if (professor == null)
                throw new InvalidOperationException("Professor not found.");
            if (professor.Role.ToString() != "Professor")
                throw new InvalidOperationException("The user is not a Professor.");

            // Asignar profesor
            commission.ProfessorId = professorId.Value;
            commission.Status = "Activo";

            await _context.SaveChangesAsync(ct);
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
