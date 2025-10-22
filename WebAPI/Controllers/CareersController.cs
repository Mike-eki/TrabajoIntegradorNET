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
    [Authorize(Roles = "Admin")] // ¡Protegemos todo el controlador solo para Admins!
    public class CareersController : ControllerBase
    {
        private readonly ICareerRepository _repo;

        public CareersController(ICareerRepository repo)
        {
            _repo = repo;
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

        [HttpPost]
        public async Task<IActionResult> CreateCareer([FromBody] CareerCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var career = new Career { Name = dto.Name };
            await _repo.AddAsync(career);

            var response = new CareerResponseDto
            {
                Id = career.Id,
                Name = career.Name,
                Subjects = new List<SubjectSimpleDto>()
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