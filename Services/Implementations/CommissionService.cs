using ADO.NET;
using EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.DTOs;
using Services.Interfaces;

namespace Services.Implementations
{
    public class CommissionService : ICommissionService
    {
        private readonly AppDbContext _context;
        private readonly IUserRepository _userRepo;
        private readonly ILogger<CommissionService> _logger;

        public CommissionService(AppDbContext context, IUserRepository userRepo, ILogger<CommissionService> logger)
        {
            _context = context;
            _userRepo = userRepo;
            _logger = logger;
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
