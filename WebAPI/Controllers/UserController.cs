using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Models.Entities;
using Services.Interfaces;
using Services;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;

        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        // En producción: Obtendrías el userId del token JWT
        private const int TEST_USER_ID = 1; // Para pruebas

        public UsersController(
            IUserService userService,
            IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

    }
}
