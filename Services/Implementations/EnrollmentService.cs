using ADO.NET;
using EntityFramework;
using EntityFramework.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.DTOs;
using Models.Entities;
using Services.Interfaces;

namespace Services.Implementations
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _repo;
        private readonly AppDbContext _context;
        private readonly ILogger<EnrollmentService> _logger;
        private readonly IUserRepository _userRepo;

        public EnrollmentService(IEnrollmentRepository repo, AppDbContext context, ILogger<EnrollmentService> logger, IUserRepository userRepo)
        {
            _repo = repo;
            _context = context;
            _logger = logger;
            _userRepo = userRepo;
        }

        public async Task<EnrollmentBulkResponseDto> BulkEnrollStudentsAsync(
            EnrollmentBulkRequestDto request,
            CancellationToken ct = default)
        {
            _logger.LogInformation("Bulk enrolling {Count} students into Commission {CommissionId}",
                request.StudentIds.Count, request.CommissionId);

            // 1️⃣ Obtener comisión con sus inscripciones actuales
            var commission = await _repo.GetCommissionWithEnrollmentsAsync(request.CommissionId, ct);
            if (commission == null)
                throw new InvalidOperationException("Comisión no encontrada.");

            int capacity = commission.Capacity;
            int currentCount = commission.Enrollments.Count;
            int remainingSeats = capacity - currentCount;

            // 2️⃣ Validaciones
            if (remainingSeats <= 0)
                throw new InvalidOperationException("La comisión no tiene cupos disponibles.");

            var alreadyEnrolled = commission.Enrollments.Select(e => e.StudentId).ToHashSet();
            var newStudentIds = request.StudentIds.Where(id => !alreadyEnrolled.Contains(id)).ToList();

            if (!newStudentIds.Any())
                throw new InvalidOperationException("Todos los estudiantes seleccionados ya están inscriptos.");

            if (newStudentIds.Count > remainingSeats)
                throw new InvalidOperationException($"Solo quedan {remainingSeats} cupo(s) disponibles.");

            // 3️⃣ Transacción
            using var transaction = await _context.Database.BeginTransactionAsync(ct);
            try
            {
                var newEnrollments = newStudentIds.Select(id => new Enrollment
                {
                    CommissionId = request.CommissionId,
                    StudentId = id,
                    EnrollmentDate = DateTime.UtcNow
                });

                await _repo.AddEnrollmentsAsync(newEnrollments, ct);
                await transaction.CommitAsync(ct);

                return new EnrollmentBulkResponseDto
                {
                    CreatedCount = newStudentIds.Count,
                    SkippedCount = request.StudentIds.Count - newStudentIds.Count,
                    RemainingSeats = remainingSeats - newStudentIds.Count,
                    Message = "Estudiantes inscriptos correctamente."
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(ct);
                _logger.LogError(ex, "Error en BulkEnrollStudentsAsync");
                throw;
            }
        }

        public async Task<List<EnrollmentResponseDto>> GetByCommissionIdAsync(int commissionId, CancellationToken ct = default)
        {
            _logger.LogInformation("Retrieving enrollments for commission {CommissionId}", commissionId);

            var enrollments = await _context.Enrollments
                .Where(e => e.CommissionId == commissionId)
                .ToListAsync(ct);

            return enrollments.Select(e => new EnrollmentResponseDto
            {
                Id = e.Id,
                CommissionId = e.CommissionId,
                StudentId = e.StudentId,
                Status = e.Status,
                EnrollmentDate = e.EnrollmentDate
            }).ToList();
        }

        public async Task<List<EnrollmentDetailDto>> GetAllEnrollmentsWithDetailsAsync(CancellationToken ct = default)
        {
            // 1. Obtener todas las inscripciones desde EF
            var enrollments = await _repo.GetAllEnrollmentsAsync(ct);
            if (!enrollments.Any())
                return new List<EnrollmentDetailDto>();

            // 2. Obtener TODOS los usuarios (asumiendo que son pocos)
            var allUsers = await _userRepo.GetAllAsync(ct);
            var userDict = allUsers.ToDictionary(u => u.Id, u => u);

            // 3. Mapear a DTO
            var result = enrollments.Select(e =>
            {
                userDict.TryGetValue(e.StudentId, out var user);

                return new EnrollmentDetailDto
                {
                    Id = e.Id,
                    StudentId = e.StudentId,
                    StudentFullName = user?.Fullname ?? "Desconocido",
                    StudentLegajo = user?.Legajo ?? "Sin legajo",
                    CommissionId = e.CommissionId,
                    SubjectName = e.Commission?.Subject?.Name ?? "Sin materia",
                    CycleYear = e.Commission?.CycleYear ?? 0,
                    Status = e.Status,
                    FinalGrade = e.FinalGrade,
                    EnrollmentDate = e.EnrollmentDate,
                    UnenrollmentDate = e.UnenrollmentDate
                };
            }).ToList();

            return result;
        }

    }
}
