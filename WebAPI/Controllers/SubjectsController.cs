using EntityFramework.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Models.DTOs;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Solo Admin
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectRepository _repo;
        private readonly ILogger<SubjectsController> _logger;

        public SubjectsController(ISubjectRepository repo, ILogger<SubjectsController> logger) 
        {
            _repo = repo;
            _logger = logger;
        } 


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var subjects = await _repo.GetAllAsync();

            var response = subjects.Select(s => new SubjectResponseDto
            {
                Id = s.Id,
                Name = s.Name,
                Careers = s.Careers.Select(c => new CareerSimpleDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
            });

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var subject = await _repo.GetByIdAsync(id);
            if (subject == null)
                return NotFound();

            var response = new SubjectResponseDto
            {
                Id = subject.Id,
                Name = subject.Name,
                Careers = subject.Careers.Select(c => new CareerSimpleDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SubjectSimpleDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var subject = new Subject { Name = dto.Name };
                var created = await _repo.AddAsync(subject);

                var response = new SubjectSimpleDto
                {
                    Id = created.Id,
                    Name = created.Name
                };

                return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SubjectSimpleDto dto)
        {
            try
            {
                var subject = new Subject { Id = id, Name = dto.Name };
                await _repo.UpdateAsync(subject);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("{id}/Careers")]
        public async Task<IActionResult> AssignCareers(int id, [FromBody] int[] careerIds)
        {
            try
            {
                _logger.LogInformation("Assigning careers to subject {SubjectId}: {CareerIds}", id, string.Join(", ", careerIds));
                var subject = await _repo.GetByIdAsync(id);
                if (subject == null)
                    return NotFound();

                _logger.LogInformation("Subject found: {SubjectName}", subject.Name);
                var updated = await _repo.AssignCareersToSubject(subject, careerIds);

                _logger.LogInformation("Careers assigned successfully to subject {SubjectId}", id);
                var response = new SubjectResponseDto
                {
                    Id = updated.Id,
                    Name = updated.Name,
                    Careers = updated.Careers.Select(c => new CareerSimpleDto
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
}
