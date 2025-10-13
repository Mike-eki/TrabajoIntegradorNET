using EntityFramework.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Professor,Student")] // sólo Admin o Profesor pueden gestionar inscripciones
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentRepository _repo;

        public EnrollmentsController(IEnrollmentRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var enrollments = await _repo.GetAllAsync();
            return Ok(enrollments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var enrollment = await _repo.GetByIdAsync(id);
            if (enrollment == null)
                return NotFound();

            return Ok(enrollment);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Enrollment enrollment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _repo.AddAsync(enrollment);
                return CreatedAtAction(nameof(GetById), new { id = enrollment.Id }, enrollment);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("self")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> EnrollSelf([FromBody] int commissionId)
        {
            // ✅ 1. Obtener el ID del estudiante desde el token JWT
            var userIdClaim = User.FindFirst("user_id")?.Value;
            if (userIdClaim == null)
                return Unauthorized(new { error = "User ID not found in token." });

            int studentId = int.Parse(userIdClaim);

            // ✅ 2. Crear el objeto Enrollment
            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CommissionId = commissionId
            };

            try
            {
                // ✅ 3. Validar y crear la inscripción (usa la lógica del repositorio)
                await _repo.AddAsync(enrollment);
                return Ok(new { message = "Enrollment successful.", enrollment.Id });
            }
            catch (InvalidOperationException ex)
            {
                // Error de negocio (commission inexistente o duplicado)
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                // Error inesperado
                return StatusCode(500, new { error = "Internal server error." });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Enrollment enrollment)
        {
            if (id != enrollment.Id)
                return BadRequest();

            await _repo.UpdateAsync(enrollment);
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
