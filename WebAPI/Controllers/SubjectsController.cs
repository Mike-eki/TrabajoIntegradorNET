using EntityFramework.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Models.DTOs;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Solo Admin
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectRepository _repo;

        public SubjectsController(ISubjectRepository repo) => _repo = repo;

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
        public async Task<IActionResult> Create([FromBody] SubjectCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var subject = new Subject { Name = dto.Name };
                var created = await _repo.AddAsync(subject, dto.CareerIds);

                var response = new SubjectResponseDto
                {
                    Id = created.Id,
                    Name = created.Name,
                    Careers = created.Careers.Select(c => new CareerSimpleDto
                    {
                        Id = c.Id,
                        Name = c.Name
                    })
                };

                return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SubjectCreateDto dto)
        {
            try
            {
                var subject = new Subject { Id = id, Name = dto.Name };
                await _repo.UpdateAsync(subject, dto.CareerIds);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
}
