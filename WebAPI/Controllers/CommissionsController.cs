using EntityFramework.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Models.DTOs;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Professor")] // Solo Admin y Profesores
    public class CommissionsController : ControllerBase
    {
        private readonly ICommissionRepository _repo;

        public CommissionsController(ICommissionRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var commissions = await _repo.GetAllAsync();

            var response = commissions.Select(c => new CommissionResponseDto
            {
                Id = c.Id,
                SubjectId = c.SubjectId,
                SubjectName = c.Subject.Name,
                ProfessorId = c.ProfessorId,
                CycleYear = c.CycleYear,
                DayOfWeek = c.DayOfWeek,
                StartTime = c.StartTime,
                EndTime = c.EndTime,
                Capacity = c.Capacity,
                Status = c.Status,
                Enrollments = c.Enrollments.Select(e => new EnrollmentSimpleDto
                {
                    Id = e.Id,
                    StudentId = e.StudentId,
                    Status = e.Status
                })
            });

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var commission = await _repo.GetByIdAsync(id);
            if (commission == null)
                return NotFound();

            var response = new CommissionResponseDto
            {
                Id = commission.Id,
                SubjectId = commission.SubjectId,
                SubjectName = commission.Subject.Name,
                ProfessorId = commission.ProfessorId,
                CycleYear = commission.CycleYear,
                DayOfWeek = commission.DayOfWeek,
                StartTime = commission.StartTime,
                EndTime = commission.EndTime,
                Capacity = commission.Capacity,
                Status = commission.Status,
                Enrollments = commission.Enrollments.Select(e => new EnrollmentSimpleDto
                {
                    Id = e.Id,
                    StudentId = e.StudentId,
                    Status = e.Status
                })
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CommissionCreateDto dto)
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
                Status = dto.Status
            };

            try
            {
                await _repo.AddAsync(commission);
                return CreatedAtAction(nameof(GetById), new { id = commission.Id }, commission);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CommissionCreateDto dto)
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
            existing.Status = dto.Status;

            await _repo.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
}