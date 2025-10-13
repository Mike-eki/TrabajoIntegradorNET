using EntityFramework.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
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
            return Ok(careers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCareer(int id)
        {
            var career = await _repo.GetByIdAsync(id);
            if (career == null)
            {
                return NotFound();
            }
            return Ok(career);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCareer([FromBody] Career career)
        {
            if (career == null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            await _repo.AddAsync(career);
            return CreatedAtAction(nameof(GetCareer), new { id = career.Id }, career);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCareer(int id, [FromBody] Career career)
        {
            if (id != career.Id || !ModelState.IsValid)
            {
                return BadRequest();
            }
            await _repo.UpdateAsync(career);
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