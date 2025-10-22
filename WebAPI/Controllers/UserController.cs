using ADO.NET;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.Entities;
using Models.DTOs;
using System.Security.Claims;
using System.Threading.Tasks;

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
            var (isValid, accessToken, refreshToken, role) = await _userService.ValidateUserAsync(request.Username, request.Password);
            if (!isValid)
            {
                _logger.LogWarning("Validation failed for user {Username}.", request.Username);
                return Unauthorized(new { Message = "Invalid username or password" });
            }
            _logger.LogInformation("User validation successful for {Username}.", request.Username);
            return Ok(new { IsValid = true, AccessToken = accessToken, RefreshToken = refreshToken, Role = role });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            _logger.LogInformation("Refreshing token for user.");
            var (isValid, newAccessToken, newRefreshToken) = await _userService.RefreshTokenAsync(request.RefreshToken);
            if (!isValid)
            {
                _logger.LogWarning("Refresh token validation failed.");
                return Unauthorized(new { Message = "Invalid or expired refresh token" });
            }
            _logger.LogInformation("Token refreshed successfully.");
            return Ok(new { AccessToken = newAccessToken, RefreshToken = newRefreshToken });
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserRequest request)
        {
            _logger.LogInformation("Creating user {Username} with role {Role}.", request.Username, request.Role);
            var role = UserRoleConverter.FromString(request.Role);
            var (hash, salt) = PasswordHasher.ComputeHash(request.Password);


            var user = new User
            {
                Username = request.Username,
                Legajo = request.Legajo,
                Email = request.Email,
                Fullname = request.Fullname,
                PasswordHash = hash,
                Salt = salt,
                Role = role
            };
            await _repo.CreateAsync(user);


            _logger.LogInformation("User {Username} created with ID {Id} and role {Role}.", user.Username, user.Id, UserRoleConverter.ToString(user.Role));
            return Ok(new { user.Id, user.Username, user.Legajo, user.Email, user.Fullname, Role = UserRoleConverter.ToString(user.Role) });
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
            return Ok(new { user.Id, user.Username, Role = UserRoleConverter.ToString(user.Role), user.Email, user.Fullname, user.Legajo });
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
            user.Email = request.Email;
            user.Legajo = request.Legajo;
            user.Fullname = request.Fullname;
            await _repo.UpdateAsync(user);
            _logger.LogInformation("User {Username} updated with ID {Id} and role {Role}.", user.Username, user.Id, UserRoleConverter.ToString(user.Role));
            return Ok(new { user.Id, user.Username, Role = UserRoleConverter.ToString(user.Role), user.Email, user.Fullname, user.Legajo });
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

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation("Starting logout process.");
            var claims = User.Claims.Select(c => $"{c.Type}: {c.Value}").ToList();
            _logger.LogInformation("User Claims in Logout: {Claims}", string.Join(", ", claims));

            var userIdClaim = User.FindFirst("user_id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                _logger.LogWarning("Logout failed: Invalid user ID claim '{UserIdClaim}'.", userIdClaim ?? "null");
                return BadRequest(new { Message = "Invalid token" });
            }

            var accessToken = Request.Headers.Authorization.ToString().Replace("Bearer ", "").Trim();
            if (string.IsNullOrEmpty(accessToken))
            {
                _logger.LogWarning("Logout failed: No access token provided in Authorization header.");
                return BadRequest(new { Message = "No access token provided" });
            }

            await _userService.LogoutAsync(userId, accessToken);
            _logger.LogInformation("User ID {UserId} logged out successfully.", userId);
            return Ok(new { Message = "Logout successful" });
        }
    }

}