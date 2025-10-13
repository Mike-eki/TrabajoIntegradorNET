using EntityFramework.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;

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
            return Ok(commissions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var commission = await _repo.GetByIdAsync(id);
            if (commission == null)
                return NotFound();
            return Ok(commission);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Commission commission)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _repo.AddAsync(commission);
            return CreatedAtAction(nameof(GetById), new { id = commission.Id }, commission);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Commission commission)
        {
            if (id != commission.Id)
                return BadRequest();

            await _repo.UpdateAsync(commission);
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