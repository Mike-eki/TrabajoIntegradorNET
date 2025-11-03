using ADO.NET;
using EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.DTOs;
using Models.Enums;
using Services.Interfaces;
using EntityFramework.Repositories;

namespace Services.Implementations
{
    public class CommissionService : ICommissionService
    {
        private readonly AppDbContext _context;
        private readonly ICommissionRepository _commissionRepo;
        private readonly ISubjectRepository _subjectRepo;
        private readonly IUserRepository _userRepo;
        private readonly ILogger<CommissionService> _logger;

        public CommissionService(AppDbContext context, IUserRepository userRepo, ILogger<CommissionService> logger
, ICommissionRepository commissionRepo, ISubjectRepository subjectRepo)
        {
            _context = context;
            _userRepo = userRepo;
            _logger = logger;
            _commissionRepo = commissionRepo;
            _subjectRepo = subjectRepo;
        }

        public async Task<List<UnassignedCommissionDto>> GetUnassignedCommissionsForProfessorAsync(int professorId, CancellationToken ct)
        {
            // 1. Verificar que el usuario es un profesor válido
            var professor = await _userRepo.GetByIdAsync(professorId, ct);
            if (professor == null || professor.Role != UserRole.Professor)
                throw new InvalidOperationException("Usuario no es un profesor válido");

            // 2. Obtener comisiones sin profesor
            var unassignedCommissions = await _commissionRepo.GetUnassignedCommissionsAsync(ct);

            // 3. Obtener materias y otros datos relacionados
            var result = new List<UnassignedCommissionDto>();
            foreach (var commission in unassignedCommissions)
            {
                var subject = await _subjectRepo.GetByIdAsync(commission.SubjectId);
                result.Add(new UnassignedCommissionDto
                {
                    Id = commission.Id,
                    SubjectName = subject?.Name ?? "Materia desconocida",
                    CycleYear = commission.CycleYear,
                    DayOfWeek = commission.DayOfWeek,
                    StartTime = commission.StartTime.ToString(@"hh\:mm"),
                    EndTime = commission.EndTime.ToString(@"hh\:mm"),
                    Capacity = commission.Capacity,
                    EnrolledCount = commission.Enrollments.Count,
                    AvailableSeats = commission.Capacity - commission.Enrollments.Count,
                    Status = commission.Status
                });
            }

            return result;
        }

        public async Task<CommissionWithProfessorDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var commission = await _context.Commissions
                .Include(c => c.Subject)
                .FirstOrDefaultAsync(c => c.Id == id, ct);

            if (commission == null)
                return null;

            string? professorName = null;
            if (commission.ProfessorId.HasValue)
            {
                var professor = await _userRepo.GetByIdAsync(commission.ProfessorId.Value, ct);
                professorName = professor?.Fullname ?? "(No encontrado)";
            }

            return new CommissionWithProfessorDto
            {
                Id = commission.Id,
                SubjectId = commission.SubjectId,
                SubjectName = commission.Subject?.Name ?? "Desconocida",
                ProfessorId = commission.ProfessorId,
                ProfessorName = professorName,
                CycleYear = commission.CycleYear,
                DayOfWeek = commission.DayOfWeek,
                StartTime = ScheduleHelper.FormatTimeSpan(commission.StartTime),
                EndTime = ScheduleHelper.FormatTimeSpan(commission.EndTime),
                Capacity = commission.Capacity,
                Status = commission.Status
            };
        }

        public async Task<int> GetEnrollmentAmountOfOneCommission(int commissionId, CancellationToken ct = default)
        {
            return await _context.Enrollments.CountAsync(e => e.CommissionId == commissionId, ct);
        }
    }
}
