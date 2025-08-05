using DTOs;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.Entities;
using Services;
using Services.Interfaces;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {

        private readonly ILogger<CourseController> _logger;

        private readonly IAcademicService _academicService;

        public CourseController(IAcademicService academicService, ILogger<CourseController> logger)
        {
            _logger = logger;

            _academicService = academicService;

        }


        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var courses = _academicService.GetCourses();

                List<CourseDTO> coursesDTO = courses.Select(c => new CourseDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    AcademicPeriod = c.AcademicPeriod,
                    CurriculumPlan = c.CurricularPlan,
                    SpecialtiesLinked = c.SpecialtiesLinked

                }).ToList();

                return Ok(new { courses = coursesDTO, message = "Cursos obtenidos con exito" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener cursos");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }


        [HttpPut("{id}")]
        public IActionResult Put([FromBody] CourseDTO courseDTO)
        {
            try
            {
                if (courseDTO == null || courseDTO.Id <= 0)
                {
                    return BadRequest(new { message = "Datos inválidos" });
                }

                Course course = new Course
                {
                    Id = courseDTO.Id,
                    Name = courseDTO.Name,
                    AcademicPeriod = courseDTO.AcademicPeriod,
                    CurricularPlan = courseDTO.CurriculumPlan,
                    SpecialtiesLinked = courseDTO.SpecialtiesLinked
                };

                _academicService.UpdateCourse(course);

                return Ok(new { message = "Curso actualizado con éxito" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar curso");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "ID inválido" });
                }
                
                _academicService.DeleteCourse(id);

                return Ok(new { message = "Curso eliminado con éxito" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar curso");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpPost]

        public IActionResult Post([FromBody] CourseDTO courseDTO)
        {
            try
            {
                if (courseDTO == null || string.IsNullOrWhiteSpace(courseDTO.Name))
                {
                    return BadRequest(new { message = "Datos inválidos" });
                }
                Course course = new Course
                {
                    Name = courseDTO.Name,
                    AcademicPeriod = courseDTO.AcademicPeriod,
                    CurricularPlan = courseDTO.CurriculumPlan,
                    SpecialtiesLinked = courseDTO.SpecialtiesLinked
                };
                _academicService.CreateCourse(course);
                return Ok(new { message = "Curso creado con éxito" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear curso");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        //// POST api/<CourseController>
        //[HttpPost]
        //public IActionResult Post([FromBody] string value)
        //{
        //}

        //// PUT api/<CourseController>/5
        //[HttpPut("{id}")]
        //public IActionResult Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<CourseController>/5
        //[HttpDelete("{id}")]
        //public IActionResult Delete(int id)
        //{
        //}
    }

}
