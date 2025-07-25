using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.Entities;
using Services;
using Services.Interfaces;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;

        //private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public UsersController(IAuthService authService, ILogger<UsersController> logger)
        {
            _logger = logger;

            //_userService = userService;
            _authService = authService;
        }

        // POST api/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO dto)
        {
            // 1. Validar formato de datos (atributos [Required] en LoginDTO)
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 2. Autenticar usando el servicio
            var user = _authService.Authenticate(dto.Username, dto.Password);

            // 3. Manejar resultado
            if (user == null)
                return Unauthorized(new { message = "Credenciales inválidas" });

            // 5. Devolver respuesta exitosa
            return Ok(new { message = "Login exitoso" });
        }

    }
}
