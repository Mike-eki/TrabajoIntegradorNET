using Microsoft.Data.SqlClient;
using Models.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ADO.NET
{
    public sealed class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly DbConnectionFactory _factory;

        public RefreshTokenRepository(DbConnectionFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public async Task CreateAsync(RefreshToken token, CancellationToken ct = default)
        {
            const string sql = @"INSERT INTO dbo.RefreshTokens (UserId, Token, ExpiresAt, IsRevoked, CreatedAt) 
                                OUTPUT INSERTED.Id 
                                VALUES (@UserId, @Token, @ExpiresAt, @IsRevoked, @CreatedAt);";
            await using var conn = _factory.CreateApp();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@UserId", token.UserId);
            cmd.Parameters.AddWithValue("@Token", token.Token);
            cmd.Parameters.AddWithValue("@ExpiresAt", token.ExpiresAt);
            cmd.Parameters.AddWithValue("@IsRevoked", token.IsRevoked);
            cmd.Parameters.AddWithValue("@CreatedAt", token.CreatedAt);
            await conn.OpenAsync(ct);
            token.Id = (int)await cmd.ExecuteScalarAsync(ct);
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct = default)
        {
            const string sql = @"SELECT Id, UserId, Token, ExpiresAt, IsRevoked, CreatedAt 
                                FROM dbo.RefreshTokens 
                                WHERE Token = @Token;";
            await using var conn = _factory.CreateApp();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Token", token);
            await conn.OpenAsync(ct);
            await using var rdr = await cmd.ExecuteReaderAsync(ct);
            if (!await rdr.ReadAsync(ct)) return null;
            return new RefreshToken
            {
                Id = rdr.GetInt32(0),
                UserId = rdr.GetInt32(1),
                Token = rdr.GetString(2),
                ExpiresAt = rdr.GetDateTime(3),
                IsRevoked = rdr.GetBoolean(4),
                CreatedAt = rdr.GetDateTime(5)
            };
        }

        public async Task RevokeAsync(string token, CancellationToken ct = default)
        {
            const string sql = @"UPDATE dbo.RefreshTokens 
                                SET IsRevoked = 1 
                                WHERE Token = @Token;";
            await using var conn = _factory.CreateApp();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Token", token);
            await conn.OpenAsync(ct);
            await cmd.ExecuteNonQueryAsync(ct);
        }

        public async Task RevokeAllByUserIdAsync(int userId, CancellationToken ct = default)
        {
            const string sql = @"UPDATE dbo.RefreshTokens 
                                SET IsRevoked = 1 
                                WHERE UserId = @UserId;";
            await using var conn = _factory.CreateApp();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);
            await conn.OpenAsync(ct);
            await cmd.ExecuteNonQueryAsync(ct);
        }
    }
}