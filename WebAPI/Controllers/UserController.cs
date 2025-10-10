using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using ADO.NET;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace MyApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IUserRepository _repo;
        private readonly ILogger<UsersController> _logger;

        public UsersController(UserService userService, IUserRepository repo, ILogger<UsersController> logger)
        {
            _userService = userService;
            _repo = repo;
            _logger = logger;
        }

        [HttpPost("validate")]
        public async Task<IActionResult> ValidateUser([FromBody] LoginRequest request)
        {
            _logger.LogInformation("Validating user {Username}.", request.Username);
            var (isValid, token) = await _userService.ValidateUserAsync(request.Username, request.Password);
            if (!isValid)
            {
                _logger.LogWarning("Validation failed for user {Username}.", request.Username);
                return Unauthorized(new { Message = "Invalid username or password" });
            }
            _logger.LogInformation("User validation successful for {Username}.", request.Username);
            return Ok(new { IsValid = true, Token = token });
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserRequest request)
        {
            _logger.LogInformation("Creating user {Username} with role {Role}.", request.Username, request.Role);
            var role = UserRoleConverter.FromString(request.Role); // Convertir string a enum
            var (hash, salt) = PasswordHasher.ComputeHash(request.Password);
            var user = new User
            {
                Username = request.Username,
                PasswordHash = hash,
                Salt = salt,
                Role = role
            };
            await _repo.CreateAsync(user);
            _logger.LogInformation("User {Username} created with ID {Id} and role {Role}.", user.Username, user.Id, UserRoleConverter.ToString(user.Role));
            return Ok(new { user.Id, user.Username, Role = UserRoleConverter.ToString(user.Role) });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            _logger.LogInformation("Retrieving user with ID {Id}.", id);
            var user = await _repo.GetByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {Id} not found.", id);
                return NotFound();
            }
            _logger.LogInformation("User {Username} retrieved with ID {Id}.", user.Username, user.Id);
            return Ok(new { user.Id, user.Username, Role = UserRoleConverter.ToString(user.Role) });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserRequest request)
        {
            _logger.LogInformation("Updating user with ID {Id} to role {Role}.", id, request.Role);
            var user = await _repo.GetByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {Id} not found.", id);
                return NotFound();
            }
            var role = UserRoleConverter.FromString(request.Role); // Convertir string a enum
            var (hash, salt) = PasswordHasher.ComputeHash(request.Password);
            user.Username = request.Username;
            user.PasswordHash = hash;
            user.Salt = salt;
            user.Role = role;
            await _repo.UpdateAsync(user);
            _logger.LogInformation("User {Username} updated with ID {Id} and role {Role}.", user.Username, user.Id, UserRoleConverter.ToString(user.Role));
            return Ok(new { user.Id, user.Username, Role = UserRoleConverter.ToString(user.Role) });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            _logger.LogInformation("Deleting user with ID {Id}.", id);
            var user = await _repo.GetByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {Id} not found.", id);
                return NotFound();
            }
            await _repo.DeleteAsync(id);
            _logger.LogInformation("User with ID {Id} deleted.", id);
            return NoContent();
        }
    }

    public record LoginRequest(string Username, string Password);
    public record UserRequest(string Username, string Password, string Role = "Student"); // Mantiene string para compatibilidad con JSON
}