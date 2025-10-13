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

        private readonly ICommissionRepository _commissionRepo;
        public EnrollmentsController(IEnrollmentRepository repo, ICommissionRepository commissionRepo)
        {
            _repo = repo;
            _commissionRepo = commissionRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var enrollments = await _repo.GetAllAsync(includeWithdrawn: false);
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
        public async Task<IActionResult> Create([FromBody] EnrollmentCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var enrollment = new Enrollment
            {
                StudentId = dto.StudentId,
                CommissionId = dto.CommissionId
            };

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

        [HttpDelete("self/{commissionId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> UnenrollSelf(int commissionId)
        {
            // ✅ Obtener StudentId desde el token
            var userIdClaim = User.FindFirst("user_id")?.Value;
            if (userIdClaim == null)
                return Unauthorized(new { error = "User ID not found in token." });

            int studentId = int.Parse(userIdClaim);

            // ✅ Buscar la inscripción activa
            var enrollment = (await _repo.GetAllAsync())
                .FirstOrDefault(e => e.StudentId == studentId
                                  && e.CommissionId == commissionId
                                  && e.Status == "ENROLLED");

            if (enrollment == null)
                return NotFound(new { error = "Enrollment not found or already withdrawn." });

            // ✅ Cambiar estado y registrar fecha de baja
            enrollment.Status = "WITHDRAWN";
            enrollment.UnenrollmentDate = DateTime.UtcNow;

            await _repo.UpdateAsync(enrollment);

            return Ok(new
            {
                message = "You have been successfully unenrolled.",
                enrollment.Id,
                enrollment.UnenrollmentDate
            });
        }

        [HttpPut("{id}/grade")]
        [Authorize(Roles = "Professor,Admin")]
        public async Task<IActionResult> SetFinalGrade(int id, [FromBody] int finalGrade)
        {
            if (finalGrade < 0 || finalGrade > 10)
                return BadRequest(new { error = "Final grade must be between 0 and 10." });

            var enrollment = new Enrollment { Id = id, FinalGrade = finalGrade };
            try
            {
                await _repo.UpdateAsync(enrollment);
                return Ok(new { message = "Grade updated successfully." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { error = "Enrollment not found." });
            }
        }

        [HttpGet("my")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetMyEnrollments(
    [FromQuery] string? status = null,
    [FromQuery] int? cycleYear = null)
        {
            // ✅ Obtener el ID del estudiante desde el token
            var userIdClaim = User.FindFirst("user_id")?.Value;
            if (userIdClaim == null)
                return Unauthorized(new { error = "User ID not found in token." });

            int studentId = int.Parse(userIdClaim);

            // ✅ Obtener todas las inscripciones del estudiante (incluidas las retiradas)
            var enrollments = await _repo.GetAllAsync(includeWithdrawn: true);

            var myEnrollments = enrollments
                .Where(e => e.StudentId == studentId);

            // 🧩 Filtro por estado (si se envía en query)
            if (!string.IsNullOrEmpty(status))
                myEnrollments = myEnrollments.Where(e =>
                    e.Status.Equals(status, StringComparison.OrdinalIgnoreCase));

            // 🧩 Filtro por ciclo lectivo (si se envía en query)
            if (cycleYear.HasValue)
                myEnrollments = myEnrollments.Where(e => e.Commission.CycleYear == cycleYear.Value);

            // ✅ Mapear al DTO
            var response = myEnrollments.Select(e => new EnrollmentStudentDto
            {
                EnrollmentId = e.Id,
                SubjectName = e.Commission.Subject.Name,
                CommissionDay = e.Commission.DayOfWeek,
                StartTime = e.Commission.StartTime.ToString(@"hh\:mm"),
                EndTime = e.Commission.EndTime.ToString(@"hh\:mm"),
                CycleYear = e.Commission.CycleYear,
                Status = e.Status,
                FinalGrade = e.FinalGrade
            });

            return Ok(response);
        }

        [HttpGet("my-commissions")]
        [Authorize(Roles = "Professor")]
        public async Task<IActionResult> GetMyCommissions(
        [FromQuery] int? cycleYear = null,
        [FromQuery] int? subjectId = null)
        {

            // ✅ Obtener ID del profesor desde el token
            var userIdClaim = User.FindFirst("user_id")?.Value;
            if (userIdClaim == null)
                return Unauthorized(new { error = "User ID not found in token." });

            int professorId = int.Parse(userIdClaim);

            // ✅ Obtener comisiones del profesor
            var commissions = (await _commissionRepo.GetAllAsync())
                .Where(c => c.ProfessorId == professorId);

            // 🧩 Aplicar filtros opcionales
            if (cycleYear.HasValue)
                commissions = commissions.Where(c => c.CycleYear == cycleYear.Value);

            if (subjectId.HasValue)
                commissions = commissions.Where(c => c.SubjectId == subjectId.Value);

            var result = commissions
                .Select(c => new ProfessorCommissionDto
                {
                    CommissionId = c.Id,
                    SubjectName = c.Subject.Name,
                    CycleYear = c.CycleYear,
                    DayOfWeek = c.DayOfWeek,
                    StartTime = c.StartTime.ToString(@"hh\:mm"),
                    EndTime = c.EndTime.ToString(@"hh\:mm"),
                    Capacity = c.Capacity,
                    Students = c.Enrollments.Select(e => new StudentEnrollmentDto
                    {
                        EnrollmentId = e.Id,
                        StudentId = e.StudentId,
                        Status = e.Status,
                        FinalGrade = e.FinalGrade
                    })
                });

            return Ok(result);
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

        // DTO para crear inscripción (solo Admin/Profesor)
        public class EnrollmentCreateDto
        {
            public int StudentId { get; set; }
            public int CommissionId { get; set; }
        }

        // DTO para devolver inscripción
        public class EnrollmentResponseDto
        {
            public int Id { get; set; }
            public int StudentId { get; set; }
            public int CommissionId { get; set; }
            public string SubjectName { get; set; } = null!;
            public DateTime EnrollmentDate { get; set; }
            public int? FinalGrade { get; set; }
            public string Status { get; set; } = null!;
        }

        public class EnrollmentStudentDto
        {
            public int EnrollmentId { get; set; }
            public string SubjectName { get; set; } = null!;
            public string CommissionDay { get; set; } = null!;
            public string StartTime { get; set; } = null!;
            public string EndTime { get; set; } = null!;
            public int CycleYear { get; set; }
            public string Status { get; set; } = null!;
            public int? FinalGrade { get; set; }
        }

        public class ProfessorCommissionDto
        {
            public int CommissionId { get; set; }
            public string SubjectName { get; set; } = null!;
            public int CycleYear { get; set; }
            public string DayOfWeek { get; set; } = null!;
            public string StartTime { get; set; } = null!;
            public string EndTime { get; set; } = null!;
            public int Capacity { get; set; }
            public IEnumerable<StudentEnrollmentDto> Students { get; set; } = new List<StudentEnrollmentDto>();
        }

        public class StudentEnrollmentDto
        {
            public int EnrollmentId { get; set; }
            public int StudentId { get; set; }
            public string? StudentName { get; set; }  // opcional, si tenés el nombre del usuario
            public string Status { get; set; } = null!;
            public int? FinalGrade { get; set; }
        }
    }
}
