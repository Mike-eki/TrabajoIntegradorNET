using ADO.NET;
using Azure.Core;
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
        private readonly ICareerRepository _careerRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly AppDbContext _context;
        private readonly ILogger<EnrollmentService> _logger;
        private readonly IUserRepository _userRepo;

        public EnrollmentService(IEnrollmentRepository repo, AppDbContext context, ILogger<EnrollmentService> logger, IUserRepository userRepo, ICareerRepository careerRepository, ISubjectRepository subjectRepository)
        {
            _repo = repo;
            _context = context;
            _logger = logger;
            _userRepo = userRepo;
            _careerRepository = careerRepository;
            _subjectRepository = subjectRepository;
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

        public async Task SelfEnrollAsync(int studentId, int commissionId, CancellationToken ct = default)
        {
            // 1. Verificar que el estudiante exista y sea "Student"
            var student = await _userRepo.GetByIdAsync(studentId, ct);
            if (student == null || UserRoleConverter.ToString(student.Role) != "Student")
                throw new InvalidOperationException("Estudante no encontrado o no es un estudiante válido.");

            // 2. Verificar que la comisión exista y esté "Activa"
            var commission = await _repo.GetCommissionWithEnrollmentsAsync(commissionId, ct);
            if (commission == null)
                throw new InvalidOperationException("Comisión no encontrada.");

            if (commission.Status != "Activo")
                throw new InvalidOperationException("La comisión no está activa.");

            // 3. Verificar cupo disponible
            if (commission.Enrollments.Count >= commission.Capacity)
                throw new InvalidOperationException("No hay cupos disponibles en esta comisión.");

            // 4. Verificar que no esté ya inscripto
            var existingEnrollment = commission.Enrollments
                .FirstOrDefault(e => e.StudentId == studentId && e.Status == "Inscripto");
            if (existingEnrollment != null)
                throw new InvalidOperationException("El estudiante ya está inscripto en esta comisión.");

            // 5. Crear la inscripción
            var enrollment = new Enrollment
            {
                StudentId = student.Id,
                CommissionId = commission.Id,
                EnrollmentDate = DateTime.UtcNow,
                Status = "Inscripto"
            };

            await _repo.AddAsync(enrollment);
        }

        public async Task<List<int>> GetUserCommissionIdsAsync(int userId, CancellationToken ct = default)
        {
            var enrollments = await _context.Enrollments
                .Where(e => e.StudentId == userId)
                .ToListAsync(ct);

            return enrollments.Select(e => e.CommissionId).ToList();
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

        public async Task<List<AcademicStatusDto>> GetAcademicStatusByCareerAsync(
    int userId,
    int careerId,
    CancellationToken ct = default)
        {
            // ✅ 1. Obtener todas las materias de la carrera
            var subjectIds = await _careerRepository.GetSubjectIdsForCareerAsync(careerId, ct);
            if (subjectIds == null || !subjectIds.Any())
                return new List<AcademicStatusDto>();

            // ✅ 2. Obtener todas las inscripciones válidas del usuario
            var validEnrollments = await _repo.GetValidAcademicEnrollmentsAsync(userId, subjectIds, ct);

            // ✅ 3. Para cada materia, obtener la inscripción más relevante
            var result = new List<AcademicStatusDto>();

            foreach (var subjectId in subjectIds)
            {
                var subject = await _subjectRepository.GetByIdAsync(subjectId);
                if (subject == null) continue;

                // Filtrar inscripciones para esta materia
                var subjectEnrollments = validEnrollments
                    .Where(e => e.SubjectId == subjectId)
                    .OrderByDescending(e => e.EnrollmentDate) // Más reciente primero
                    .ToList();

                // Tomar la primera con nota válida
                var relevantEnrollment = subjectEnrollments
                    .FirstOrDefault(e => e.FinalGrade.HasValue && e.FinalGrade >= 0 && e.FinalGrade <= 10);

                result.Add(new AcademicStatusDto
                {
                    SubjectId = subjectId,
                    SubjectName = subject.Name,
                    FinalGrade = relevantEnrollment?.FinalGrade
                });
            }

            return result;
        }

    }
}
