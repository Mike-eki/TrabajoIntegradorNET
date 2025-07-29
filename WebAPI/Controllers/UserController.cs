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

        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public UsersController(IAuthService authService, IUserService userService, ILogger<UsersController> logger)
        {
            _logger = logger;

            _userService = userService;
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

            // 4. Preparar respuesta
            UserDTO userDTO = new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Name = user.Name,
                RoleName = user.RoleName,
            };

            // 5. Devolver respuesta exitosa
            return Ok(new { user = userDTO, message = $"Login exitoso." });
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUserDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = _userService.RegisterUser(dto);

                RegisterUserDTO okDto = new RegisterUserDTO
                {
                    Username = user.Username,
                    Email = user.Email,
                    Name = user.Name,
                    RoleName = user.RoleName
                };

                return CreatedAtAction(nameof(Register), okDto, new {message = "Usuario registrado"});

            }
            catch (InvalidOperationException ex)
            {
                // 5. Errores de negocio (ej: "Nombre de usuario ya existe")
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                // 6. Argumentos inválidos (ej: "Rol no válido")
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar usuario");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
            
        }
    }
}
