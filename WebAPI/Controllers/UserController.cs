using ADO.NET;
using EntityFramework.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.DTOs;
using Models.Entities;
using Models.Enums;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ICareerRepository _careerRepo;
        private readonly IUserRepository _repo;
        private readonly ILogger<UsersController> _logger;

        public UsersController(UserService userService, IUserRepository repo, ILogger<UsersController> logger, ICareerRepository careerRepo)
        {
            _userService = userService;
            _repo = repo;
            _logger = logger;
            _careerRepo = careerRepo;
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
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto request, CancellationToken ct = default)
        {
            if (await _repo.ExistsAsync(request.Username, ct))
            {
                return Conflict(new { message = "Username already exists." });
            }

            _logger.LogInformation("Creating user {Username} with role {Role}.", request.Username, request.Role);
            var role = UserRoleConverter.FromString(request.Role);
            var (hash, salt) = PasswordHasher.ComputeHash(request.Password);


            var user = new User
            {
                Username = request.Username,
                Legajo = request.Legajo,
                Email = request.Email,
                Fullname = request.FullName,
                PasswordHash = hash,
                Salt = salt,
                Role = role
            };
            


            await _repo.CreateAsync(user, ct);

            // ✅ Solo asociar carreras si el rol es Student
            if (user.Role == UserRole.Student && request.CareerIds != null && request.CareerIds.Any())
            {
                await _repo.UpdateUserCareersAsync(user.Id, request.CareerIds, ct);
                _logger.LogInformation("Associated {Count} career(s) to student {Username}.", request.CareerIds.Count, request.Username);
            }
            else if (request.CareerIds != null && request.CareerIds.Any())
            {
                _logger.LogInformation("User {Username} is not a Student — ignoring {Count} career(s).", request.Username, request.CareerIds.Count);
            }


            _logger.LogInformation("User {Username} created with ID {Id} and role {Role}.", user.Username, user.Id, UserRoleConverter.ToString(user.Role));
            return Ok(new { user.Id, user.Username, user.Legajo, user.Email, user.Fullname, Role = UserRoleConverter.ToString(user.Role) });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id, CancellationToken ct)
        {
            _logger.LogInformation("Retrieving user with ID {Id}.", id);
            var user = await _repo.GetByIdAsync(id, ct);
            if (user == null)
            {
                _logger.LogWarning("User with ID {Id} not found.", id);
                return NotFound();
            }
            _logger.LogInformation("User {Username} retrieved with ID {Id}.", user.Username, user.Id);
            var response = new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Legajo = user.Legajo,
                Email = user.Email,
                FullName = user.Fullname,
                Role = UserRoleConverter.ToString(user.Role)
            };

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers(string? role = null, int? subjectId = null, CancellationToken ct = default)
        {
            try
            {
                _logger.LogInformation("Retrieving users. Role: {Role}, SubjectId: {SubjectId}", role ?? "All", subjectId);

                List<User> users;

                // 🔹 Si tiene subjectId y rol Student → usar el nuevo método optimizado
                if (subjectId.HasValue && role?.Equals("Student", StringComparison.OrdinalIgnoreCase) == true)
                {
                    users = await _repo.GetStudentsBySubjectIdAsync(subjectId.Value, ct);
                }
                else
                {
                    users = await _repo.GetAllAsync(ct);

                    if (!string.IsNullOrEmpty(role))
                    {
                        UserRoleConverter.FromString(role);
                        users = users
                            .Where(u => u.Role.ToString().Equals(role, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                    }
                }

                var dtoList = users.Select(u => new UserResponseDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Legajo = u.Legajo,
                    Email = u.Email,
                    FullName = u.Fullname,
                    Role = u.Role.ToString()
                }).ToList();

                return Ok(dtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users");
                return StatusCode(500, new { message = "Error interno al obtener usuarios.", detail = ex.Message });
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto request, CancellationToken ct)
        {
            _logger.LogInformation("Updating user with ID {Id} to role {Role}.", id, request.Role);
            var user = await _repo.GetByIdAsync(id, ct);
            if (user == null)
            {
                _logger.LogWarning("User with ID {Id} not found.", id);
                return NotFound();
            }
            var role = UserRoleConverter.FromString(request.Role); // Convertir string a enum

            user.Role = role;
            user.Email = request.Email;
            user.Fullname = request.FullName;


            await _repo.UpdateAsync(user);

            _logger.LogInformation("User {Username} updated with ID {Id} and role {Role}.", user.Username, user.Id, UserRoleConverter.ToString(user.Role));
            return Ok(new { user.Id, user.Username, Role = UserRoleConverter.ToString(user.Role), user.Email, user.Fullname, user.Legajo });
        }

        [Authorize(Roles = "Student,Professor")]
        [HttpGet("profile")]
        public async Task<IActionResult> GetUserProfile(CancellationToken ct)
        {
            try
            {
                int userClaimId;
                bool parseSuccess = int.TryParse(User.FindFirst("user_id")?.Value, out userClaimId);
                if (!parseSuccess)
                {
                    throw new Exception("No se logro parsear correctamente el token.");
                }
                var user = await _repo.GetByIdAsync(userClaimId);

                if (user == null)
                {
                    return NotFound(new { message = "Usuario no encontrado." });
                }
                // Obtener carerras asociadas al user
                var careerIds = _repo.GetCareerIdsByUserIdAsync(user.Id, ct);
                var careersList = new List<CareerSimpleDto>();

                foreach (var careerId in careerIds.Result)
                {
                    var userCareer = await _careerRepo.GetByIdAsync(careerId);
                    if (userCareer != null)
                    {
                        careersList.Add(new CareerSimpleDto
                        {
                            Id = userCareer.Id,
                            Name = userCareer.Name,
                        });
                    }
                    else
                    {
                        throw new Exception($"No se encontro la carrera con Id {careerId}.");
                    }
                }

                var userDto = new UserProfileDto
                {
                    Email = user.Email,
                    FullName = user.Fullname,
                    Role = UserRoleConverter.ToString(user.Role),
                    Careers = careersList
                };

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener perfil del usuario");
                return StatusCode(500, new { message = "Error al obtener perfil del usuario", detail = ex.Message });
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id, CancellationToken ct)
        {
            _logger.LogInformation("Deleting user with ID {Id}.", id);
            var user = await _repo.GetByIdAsync(id, ct);
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

        [Authorize]
        [HttpPost("{id}/reset-password")]
        public async Task<IActionResult> ResetPassword(int id, [FromBody] ResetPasswordDto request, CancellationToken ct)
        {
            _logger.LogInformation("Resetting password for user ID {Id}.", id);

            var user = await _repo.GetByIdAsync(id, ct);
            if (user == null)
            {
                _logger.LogWarning("User with ID {Id} not found.", id);
                return NotFound();
            }

            var (hash, salt) = PasswordHasher.ComputeHash(request.NewPassword);
            user.PasswordHash = hash;
            user.Salt = salt;

            await _repo.UpdateAsync(user);
            _logger.LogInformation("Password reset succesfully for user ID {Id}.", id);

            return Ok(new { Message = "Passworded reset succesfully" });

        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{userId}/careers")]
        public async Task<IActionResult> GetUserCareers(int userId, CancellationToken ct = default)
        {
            _logger.LogInformation("Retrieving careers for user {UserId}", userId);

            var careerIds = await _repo.GetCareerIdsByUserIdAsync(userId, ct);

            if (careerIds == null || careerIds.Count == 0)
            {
                _logger.LogWarning("No careers found for user {UserId}", userId);
                return Ok(new List<object>()); // vacío
            }

            var careers = await _careerRepo.GetAllAsync(); // EF
            var userCareers = careers
                .Where(c => careerIds.Contains(c.Id))
                .Select(c => new { c.Id, c.Name })
                .ToList();

            return Ok(userCareers);
        }

        [Authorize(Roles = "Admin,Student,Professor")]
        [HttpPut("{userId}/careers")]
        public async Task<IActionResult> UpdateUserCareers(int userId, [FromBody] List<int> careerIds, CancellationToken ct = default)
        {
            _logger.LogInformation("Updating careers for user {UserId}", userId);

            // Obtener usuario actual desde ADO.NET (sin exponer la contraseña)
            var user = await _repo.GetByIdAsync(userId, ct);
            if (user == null)
            {
                _logger.LogWarning("User {UserId} not found.", userId);
                return NotFound(new { message = "User not found." });
            }

            // Solo estudiantes pueden tener carreras asociadas
            if (!user.Role.Equals(UserRole.Student))
            {
                _logger.LogInformation("User {UserId} is not a Student. Career associations will be ignored.", userId);
                return Ok(new { message = "No changes applied. Only students can be associated with careers." });
            }

            // Si llega acá, es un estudiante → aplicar los cambios
            await _repo.UpdateUserCareersAsync(userId, careerIds, ct);

            _logger.LogInformation("Updated {Count} career(s) for student {UserId}.", careerIds.Count, userId);
            return NoContent();
        }




    }

}