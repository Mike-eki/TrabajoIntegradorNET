using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Models.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ADO.NET
{
    public sealed class UserService
    {
        private readonly IUserRepository _repo;
        private readonly IRefreshTokenRepository _refreshTokenRepo;
        private readonly IRevokedAccessTokenRepository _revokedAccessTokenRepo;
        private readonly ILogger<UserService> _logger;
        private readonly IConfiguration _config;

        public UserService(IUserRepository repo, IRefreshTokenRepository refreshTokenRepo, IRevokedAccessTokenRepository revokedAccessTokenRepo, ILogger<UserService> logger, IConfiguration config)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _refreshTokenRepo = refreshTokenRepo ?? throw new ArgumentNullException(nameof(refreshTokenRepo));
            _revokedAccessTokenRepo = revokedAccessTokenRepo ?? throw new ArgumentNullException(nameof(revokedAccessTokenRepo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task<(bool IsValid, string? AccessToken, string? RefreshToken)> ValidateUserAsync(string username, string plainPassword, CancellationToken ct = default)
        {
            var user = await _repo.GetByUsernameAsync(username, ct);
            if (user == null)
            {
                _logger.LogWarning("Validation failed: User {Username} not found.", username);
                return (false, null, null);
            }

            bool isValid;

            // Caso legacy: Si Salt es vacío, usa verificación legacy (sal estática)
            if (string.IsNullOrEmpty(user.Salt))
            {
                isValid = PasswordHasher.VerifyLegacy(plainPassword, user.PasswordHash);

                if (isValid)
                {
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
                isValid = PasswordHasher.Verify(plainPassword, user.PasswordHash, user.Salt);
            }

            if (!isValid)
            {
                _logger.LogWarning("Validation failed for user {Username}: Invalid password.", username);
                return (false, null, null);
            }

            // Generate access token and refresh token
            var accessToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();
            var refreshTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(double.Parse(_config["Jwt:RefreshTokenExpiryDays"])),
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false
            };
            await _refreshTokenRepo.CreateAsync(refreshTokenEntity, ct);
            _logger.LogInformation("Access and refresh tokens generated for user {Username}.", username);
            return (true, accessToken, refreshToken);
        }

        public async Task<(bool IsValid, string? AccessToken, string? RefreshToken)> RefreshTokenAsync(string refreshToken, CancellationToken ct = default)
        {
            var tokenEntity = await _refreshTokenRepo.GetByTokenAsync(refreshToken, ct);
            if (tokenEntity == null || tokenEntity.IsRevoked || tokenEntity.ExpiresAt < DateTime.UtcNow)
            {
                _logger.LogWarning("Refresh token validation failed: Token invalid, revoked, or expired.");
                return (false, null, null);
            }

            var user = await _repo.GetByIdAsync(tokenEntity.UserId, ct);
            if (user == null)
            {
                _logger.LogWarning("Refresh token validation failed: User with ID {UserId} not found.", tokenEntity.UserId);
                return (false, null, null);
            }

            // Revoke old refresh token
            await _refreshTokenRepo.RevokeAsync(refreshToken, ct);

            // Generate new access token and refresh token
            var newAccessToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();
            var newRefreshTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                Token = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(double.Parse(_config["Jwt:RefreshTokenExpiryDays"])),
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false
            };
            await _refreshTokenRepo.CreateAsync(newRefreshTokenEntity, ct);
            _logger.LogInformation("New access and refresh tokens generated for user ID {UserId}.", user.Id);
            return (true, newAccessToken, newRefreshToken);
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username), // sub para username
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("user_id", user.Id.ToString()), // Claim personalizado para UserId
                new Claim(ClaimTypes.Role, UserRoleConverter.ToString(user.Role))
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:AccessTokenExpiryMinutes"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task LogoutAsync(int userId, string accessToken, CancellationToken ct = default)
        {
            _logger.LogInformation("Logging out user ID {UserId}. Revoking all refresh tokens and access token.", userId);
            await _refreshTokenRepo.RevokeAllByUserIdAsync(userId, ct);
            await _revokedAccessTokenRepo.RevokeAsync(accessToken, ct);
            _logger.LogInformation("Logout completed for user ID {UserId}.", userId);
        }
    }
}