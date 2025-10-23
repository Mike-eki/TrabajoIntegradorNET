using EntityFramework.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Models.DTOs;
using System.Threading.Tasks;

namespace MyApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CareersController : ControllerBase
    {
        private readonly ICareerRepository _repo;
        private readonly ILogger<CareersController> _logger;
        public CareersController(ICareerRepository repo, ILogger<CareersController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCareers()
        {
            var careers = await _repo.GetAllAsync();

            var response = careers.Select(c => new CareerResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Subjects = c.Subjects.Select(s => new SubjectSimpleDto
                {
                    Id = s.Id,
                    Name = s.Name
                })
            });

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCareer(int id)
        {
            var career = await _repo.GetByIdAsync(id);
            if (career == null)
                return NotFound();

            var response = new CareerResponseDto
            {
                Id = career.Id,
                Name = career.Name,
                Subjects = career.Subjects.Select(s => new SubjectSimpleDto
                {
                    Id = s.Id,
                    Name = s.Name
                })
            };

            return Ok(response);
        }

        [HttpPost("{id}/Subjects")]
        public async Task<IActionResult> AssignSubjects(int id, [FromBody] int[] subjectIds)
        {
            try
            {
                _logger.LogInformation("Assigning careers to subject {CareerId}: {SubjectIds}", id, string.Join(", ", subjectIds));
                var career = await _repo.GetByIdAsync(id);
                if (career == null)
                    return NotFound();

                _logger.LogInformation("Career found: {CareerName}", career.Name);
                var updated = await _repo.AssignSubjectsToCareer(career, subjectIds);

                _logger.LogInformation("Subjects assigned successfully to subject {CareerId}", id);
                var response = new CareerResponseDto
                {
                    Id = updated.Id,
                    Name = updated.Name,
                    Subjects = updated.Subjects.Select(c => new SubjectSimpleDto
                    {
                        Id = c.Id,
                        Name = c.Name
                    })
                };
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCareer([FromBody] CareerSimpleDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var career = new Career { Name = dto.Name };
            await _repo.AddAsync(career);

            var response = new CareerSimpleDto
            {
                Id = career.Id,
                Name = career.Name,
            };

            return CreatedAtAction(nameof(GetCareer), new { id = career.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCareer(int id, [FromBody] CareerCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            existing.Name = dto.Name;
            await _repo.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCareer(int id)
        {
            var existingCareer = await _repo.GetByIdAsync(id);
            if (existingCareer == null)
            {
                return NotFound();
            }
            await _repo.DeleteAsync(id);
            return NoContent();
        }

        
    }
}