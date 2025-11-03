using EntityFramework.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Models.Entities;
using OpenTelemetry.Trace;
using Services;
using Services.Interfaces;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommissionsController : ControllerBase
    {
        private readonly ICommissionRepository _repo;
        private readonly ICommissionService _commissionService;
        private readonly ILogger<CommissionsController> _logger;

        public CommissionsController(ICommissionRepository repo, ILogger<CommissionsController> logger, ICommissionService commissionService)
        {
            _repo = repo;
            _logger = logger;
            _commissionService = commissionService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllCommissionsWithProfessors(
            [FromQuery] string? status = null,
            CancellationToken ct = default)
        {
            try
            {
                var commissions = await _repo.GetAllCommissionsWithProfessorsAsync(ct);


                // ✅ Filtrar SOLO si se proporciona un status válido
                if (!string.IsNullOrEmpty(status))
                {
                    commissions = commissions
                        .Where(c => c.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                // ✅ Calcular cupos disponibles para TODAS las comisiones (no solo las activas)
                foreach (var commission in commissions)
                {
                    var enrollmentAmount = await _commissionService.GetEnrollmentAmountOfOneCommission(commission.Id, ct);
                    commission.AvailableAmount = commission.Capacity - enrollmentAmount;
                }

                return Ok(commissions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las comisiones.");
                return StatusCode(500, new { Message = "Error interno al obtener las comisiones.", Error = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var dto = await _commissionService.GetByIdAsync(id, ct);
            if (dto == null)
                return NotFound(new { message = "Comisión no encontrada." });

            return Ok(dto);
        }


        [Authorize(Roles = "Admin,Professor")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CommissionCreateDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var commission = new Commission
            {
                SubjectId = dto.SubjectId,
                ProfessorId = dto.ProfessorId,
                CycleYear = dto.CycleYear,
                DayOfWeek = dto.DayOfWeek,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Capacity = dto.Capacity,
                Status = dto.ProfessorId.HasValue ? dto.Status : "Pendiente"
            };

            try
            {
                // Validar horario
                ScheduleHelper.ValidateSchedule(dto.StartTime.Hours, dto.StartTime.Minutes);
                ScheduleHelper.ValidateSchedule(dto.EndTime.Hours, dto.EndTime.Minutes);

                await _repo.AddAsync(commission, ct);
                return Ok(new { Success = true, Message = "Commission added successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,Professor")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CommissionUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            existing.SubjectId = dto.SubjectId;
            existing.ProfessorId = dto.ProfessorId;
            existing.CycleYear = dto.CycleYear;
            existing.DayOfWeek = dto.DayOfWeek;
            existing.StartTime = dto.StartTime;
            existing.EndTime = dto.EndTime;
            existing.Capacity = dto.Capacity;
            existing.Status = CommissionHelper.ResolveStatus(dto.ProfessorId, dto.Status);

            await _repo.UpdateAsync(existing);
            return Ok(new {Succes = true, Message = "Succefully commission edited"});
        }

        [Authorize(Roles = "Admin, Professor")]
        [HttpPut("{id}/assign-professor")]
        public async Task<IActionResult> AssignProfessor(int id, CancellationToken ct = default)
        {
            try
            {
                int professorId;
                int.TryParse(User.FindFirst("user_id")?.Value, out professorId);
                if (string.IsNullOrEmpty(professorId.ToString()) || !int.TryParse(professorId.ToString(), out int currentUserId))
                    return Unauthorized();

                if (professorId != currentUserId)
                    return Forbid(); // 403 Forbidden

                await _repo.AssignProfessorAsync(id, professorId, ct);

                return Ok(new { Message = "Te has asignado correctamente a la comisión." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al asignar profesor a comisión");
                return StatusCode(500, new { Message = "Error interno al asignar profesor." });
            }
        }

        [Authorize(Roles = "Professor")]
        [HttpGet("unassigned")]
        public async Task<IActionResult> GetUnassignedCommissions(CancellationToken ct = default)
        {
            try
            {
                // 1. Obtener ID del profesor autenticado
                //var professorId = GetUserIdFromToken(); // ← Ya tienes este método
                int professorId;
                int.TryParse(User.FindFirst("user_id")?.Value, out professorId);
                if (string.IsNullOrEmpty(professorId.ToString()) || !int.TryParse(professorId.ToString(), out int currentUserId))
                    return Unauthorized();

                if (professorId != currentUserId)
                    return Forbid(); // 403 Forbidden

                // 2. Llamar al servicio
                var commissions = await _commissionService.GetUnassignedCommissionsForProfessorAsync(professorId, ct);

                return Ok(commissions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener comisiones sin asignar para profesor");
                return StatusCode(500, new { Message = "Error interno al obtener comisiones." });
            }
        }

        


        [Authorize(Roles = "Admin,Professor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }

    }
}