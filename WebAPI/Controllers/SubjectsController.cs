using EntityFramework.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;

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
            var resp = subjects.Select(s => new SubjectResponse(
                s.Id, s.Name, s.Careers.Select(c => new CareerDto(c.Id, c.Name))
            ));
            return Ok(resp);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var s = await _repo.GetByIdAsync(id);
            if (s == null) return NotFound();
            var resp = new SubjectResponse(s.Id, s.Name, s.Careers.Select(c => new CareerDto(c.Id, c.Name)));
            return Ok(resp);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SubjectCreateRequest req)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var subject = new Subject { Name = req.Name };
            try
            {
                var created = await _repo.AddAsync(subject, req.CareerIds);
                return CreatedAtAction(nameof(Get), new { id = created.Id }, new SubjectResponse(created.Id, created.Name, created.Careers.Select(c => new CareerDto(c.Id, c.Name))));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SubjectCreateRequest req)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var subject = new Subject { Id = id, Name = req.Name };
            try
            {
                await _repo.UpdateAsync(subject, req.CareerIds);
                return NoContent();
            }
            catch (KeyNotFoundException) { return NotFound(); }
            catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }

        // Request para crear/editar Subject
        public record SubjectCreateRequest(
            string Name,
            int[] CareerIds // <-- lista de IDs de carreras a las que pertenece
        );

        // Response
        public record SubjectResponse(
            int Id,
            string Name,
            IEnumerable<CareerDto> Careers
        );

        public record CareerDto(int Id, string Name);
    }
}
