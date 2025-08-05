using DTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialtyController : ControllerBase
    {

        private readonly ILogger<SpecialtyController> _logger;
        private readonly IAcademicService _academicService;
        public SpecialtyController(IAcademicService academicService, ILogger<SpecialtyController> logger)
        {
            _logger = logger;
            _academicService = academicService;
        }
        // GET: api/<SpecialtyController>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var specialties = _academicService.GetSpecialties();

                List<SpecialtyDTO> speciltiesDTO = specialties.Select(s => new SpecialtyDTO
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList();

                return Ok(new { specialties = speciltiesDTO, message = "Especialidades obtenidas con éxito" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener especialidades");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
            
        }

        // GET api/<SpecialtyController>/5
        //[HttpGet("{id}")]
        //public IActionResult Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<SpecialtyController>
        //[HttpPost]
        //public IActionResult Post([FromBody] string value)
        //{
        //    try
        //    {
        //        _academicService.CreateSpecialty(value);
        //        _logger.LogInformation("Especialidad creada: {SpecialtyName}", value);


        //        return Ok(new { message = "Especialidad creada con éxito" });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error al obtener especialidades");
        //        return StatusCode(500, new { message = "Error interno del servidor" });
        //    }
        //}

        // PUT api/<SpecialtyController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SpecialtyController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
