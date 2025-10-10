using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Models.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ADO.NET
{
    public sealed class UserService
    {
        private readonly IUserRepository _repo;
        private readonly ILogger<UserService> _logger;
        private readonly IConfiguration _config;

        public UserService(IUserRepository repo, ILogger<UserService> logger, IConfiguration config)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task<(bool IsValid, string? Token)> ValidateUserAsync(string username, string plainPassword, CancellationToken ct = default)
        {
            var user = await _repo.GetByUsernameAsync(username, ct);
            if (user == null)
            {
                _logger.LogWarning("Validation failed: User {Username} not found.", username);
                return (false, null);
            }

            bool isValid;

            // Caso legacy: Si Salt es vacío, usa verificación legacy (sal estática)
            if (string.IsNullOrEmpty(user.Salt))
            {
                isValid = PasswordHasher.VerifyLegacy(plainPassword, user.PasswordHash);

                if (isValid)
                {
                    // Migrar a sal única
                    _logger.LogInformation("Migrating legacy user {Username} to unique salt.", username);
                    var (newHash, newSalt) = PasswordHasher.ComputeHash(plainPassword);
                    user.PasswordHash = newHash;
                    user.Salt = newSalt;
                    await _repo.UpdateAsync(user, ct);
                    _logger.LogInformation("Migration completed for user {Username}.", username);
                }
            }
            else
            {
                // Caso normal: Usa sal única
                isValid = PasswordHasher.Verify(plainPassword, user.PasswordHash, user.Salt);
            }

            if (!isValid)
            {
                _logger.LogWarning("Validation failed for user {Username}: Invalid password.", username);
                return (false, null);
            }

            // Generate JWT
            var token = GenerateJwtToken(user);
            _logger.LogInformation("JWT generated for user {Username}.", username);
            return (true, token);
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, UserRoleConverter.ToString(user.Role)) // Convertir enum a string para JWT
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(_config["Jwt:ExpiryMinutes"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}