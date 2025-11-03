using Azure.Core;
using EntityFramework.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Entities;
using Services.Implementations;
using Services.Interfaces;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Professor,Student")] // sólo Admin o Profesor pueden gestionar inscripciones
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly IEnrollmentRepository _enrollmentRepo;
        private readonly ILogger<EnrollmentsController> _logger;
        private readonly ICommissionRepository _commissionRepo;
        public EnrollmentsController(IEnrollmentService enrollmentService, ICommissionRepository commissionRepo, ILogger<EnrollmentsController> logger, IEnrollmentRepository enrollmentRepo)
        {
            _enrollmentService = enrollmentService;
            _commissionRepo = commissionRepo;
            _enrollmentRepo = enrollmentRepo;
            _logger = logger;
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllEnrollments(CancellationToken ct = default)
        {
            // Asumiendo que tu servicio puede traer StudentName
            var enrollments = await _enrollmentService.GetAllEnrollmentsWithDetailsAsync(ct);
            return Ok(enrollments);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("bulk")]
        public async Task<IActionResult> BulkEnrollStudents(
            [FromBody] EnrollmentBulkRequestDto request,
            CancellationToken ct)
        {
            try
            {
                var result = await _enrollmentService.BulkEnrollStudentsAsync(request, ct);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Validation error: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in bulk enrollment");
                return StatusCode(500, new { message = "Error interno al inscribir estudiantes.", detail = ex.Message });
            }
        }

        [Authorize(Roles = "Student")]
        [HttpPost("self")]
        public async Task<IActionResult> SelfEnrollInCommission(
    [FromBody] SelfEnrollmentRequestDto request,
    CancellationToken ct = default)
        {

            // ✅ Validar ModelState primero
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Solicitud mal formada: {Errors}", string.Join(", ", ModelState.Values.SelectMany(v => v.Errors)));
                return BadRequest(new { message = "Solicitud mal formada", errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            int userIdClaim;
             int.TryParse(User.FindFirst("user_id")?.Value, out userIdClaim);
            if (string.IsNullOrEmpty(userIdClaim.ToString()) || !int.TryParse(userIdClaim.ToString(), out int currentUserId))
                return Unauthorized();

            if (request.StudentId != currentUserId)
                return Forbid(); // 403 Forbidden


            try
            {
                await _enrollmentService.SelfEnrollAsync(request.StudentId, request.CommissionId, ct);
                return Ok(new { message = "Inscripción exitosa." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en auto-inscripción del estudiante {StudentId} a comisión {CommissionId}",
                    request.StudentId, request.CommissionId);
                return StatusCode(500, new { message = "Error interno al inscribirse." });
            }
        }

        [Authorize]
        [HttpGet("user/{userId}/commissions")]
        public async Task<IActionResult> GetUserCommissionIds(int userId, CancellationToken ct)
        {

            List<int> commissionIds = await _enrollmentService.GetUserCommissionIdsAsync(userId, ct);
            return Ok(commissionIds);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("commission/{commissionId}")]
        public async Task<IActionResult> GetByCommission(int commissionId, CancellationToken ct)
        {
            var result = await _enrollmentService.GetByCommissionIdAsync(commissionId, ct);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("academicstatus/user/{userId}/career/{careerId}")]
        public async Task<IActionResult> GetAcademicStatusByCareer(
    int userId,
    int careerId,
    CancellationToken ct = default)
        {
            // ✅ Validar usuario autenticado
            int userIdClaim;
            int.TryParse(User.FindFirst("user_id")?.Value, out userIdClaim);
            if (string.IsNullOrEmpty(userIdClaim.ToString()) || !int.TryParse(userIdClaim.ToString(), out int currentUserId))
                return Unauthorized();

            if (userId != currentUserId)
                return Forbid(); // 403 Forbidden

            try
            {
                var status = await _enrollmentService.GetAcademicStatusByCareerAsync(userId, careerId, ct);
                return Ok(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estado académico para usuario {UserId}, carrera {CareerId}", userId, careerId);
                return StatusCode(500, new { message = "Error interno al obtener estado académico." });
            }
        }

        [HttpPut("{id}/grade")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SetFinalGrade(int id, [FromBody] GradeUpdateDto dto)
        {
            if (dto.FinalGrade.HasValue && (dto.FinalGrade < 0 || dto.FinalGrade > 10))
                return BadRequest("La nota debe estar entre 0 y 10.");

            var enrollment = await _enrollmentRepo.GetByIdAsync(id);
            if (enrollment == null)
                return NotFound();

            enrollment.FinalGrade = dto.FinalGrade;
            await _enrollmentRepo.UpdateAsync(enrollment);

            return NoContent();
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusDto dto)
        {
            if (string.IsNullOrEmpty(dto.Status) || !IsValidStatus(dto.Status))
                return BadRequest("Estado inválido.");

            var enrollment = await _enrollmentRepo.GetByIdAsync(id);
            if (enrollment == null)
                return NotFound();

            enrollment.Status = dto.Status;
            await _enrollmentRepo.UpdateAsync(enrollment);

            return NoContent();
        }

        private static bool IsValidStatus(string status) =>
            status is "Inscripto" or "Aprobado" or "Cerrado" or "Baja";

    }
}
