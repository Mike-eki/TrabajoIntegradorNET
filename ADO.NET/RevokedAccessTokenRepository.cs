using Microsoft.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging; // Nuevo

namespace ADO.NET
{
    public sealed class RevokedAccessTokenRepository : IRevokedAccessTokenRepository
    {
        private readonly DbConnectionFactory _factory;
        private readonly ILogger<RevokedAccessTokenRepository> _logger; // Nuevo

        public RevokedAccessTokenRepository(DbConnectionFactory factory, ILogger<RevokedAccessTokenRepository> logger)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task RevokeAsync(string token, CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Attempted to revoke null or empty token.");
                return;
            }
            const string sql = @"INSERT INTO dbo.RevokedAccessTokens (Token) VALUES (@Token);";
            await using var conn = _factory.CreateApp();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Token", token);
            try
            {
                await conn.OpenAsync(ct);
                await cmd.ExecuteNonQueryAsync(ct);
                _logger.LogInformation("Access token revoked successfully: {Token}", token);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Failed to revoke access token: {Token}", token);
                throw;
            }
        }

        public async Task<bool> IsRevokedAsync(string token, CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("IsRevokedAsync called with null or empty token.");
                return false;
            }
            const string sql = @"SELECT 1 FROM dbo.RevokedAccessTokens WHERE Token = @Token;";
            await using var conn = _factory.CreateApp();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Token", token);
            try
            {
                await conn.OpenAsync(ct);
                var result = await cmd.ExecuteScalarAsync(ct);
                var isRevoked = result != null;
                _logger.LogInformation("Checked token revocation: Token={Token}, IsRevoked={IsRevoked}", token, isRevoked);
                return isRevoked;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Failed to check token revocation: {Token}", token);
                throw;
            }
        }
    }
}