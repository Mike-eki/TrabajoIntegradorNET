using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Models.Entities;
using System;
using System.Data;
using System.Runtime.Intrinsics.Arm;
using System.Threading;
using System.Threading.Tasks;

namespace ADO.NET
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly DbConnectionFactory _factory;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(DbConnectionFactory factory, ILogger<UserRepository> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public async Task<bool> ExistsAsync(string username, CancellationToken ct = default)
        {
            const string sql = @"SELECT 1 FROM dbo.Users WHERE Username = @Username;";
            await using var conn = _factory.CreateApp();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Username", username);
            await conn.OpenAsync(ct);
            var result = await cmd.ExecuteScalarAsync(ct);
            return result != null;
        }

        public async Task CreateAsync(User user, CancellationToken ct = default)
        {
            const string sql = @"INSERT INTO dbo.Users (AcademicId, Username, PasswordHash, Salt, FullName, Email, Role) 
                                OUTPUT INSERTED.Id 
                                VALUES (@AcademicId, @Username, @PasswordHash, @Salt, @FullName, @Email, @Role);";
            await using var conn = _factory.CreateApp();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@AcademicId", user.AcademicId);
            cmd.Parameters.AddWithValue("@Username", user.Username);
            cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            cmd.Parameters.AddWithValue("@Salt", user.Salt);
            cmd.Parameters.AddWithValue("@FullName", user.FullName);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@Role", UserRoleConverter.ToString(user.Role)); // Convertir enum a string

            try
            {
                await conn.OpenAsync(ct);
                user.Id = (int)await cmd.ExecuteScalarAsync(ct);
                _logger.LogInformation("User created successfully: {Username}, AcademicId: {AcademicId}", user.Username, user.AcademicId);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Failed to create user: {Username}, AcademicId: {AcademicId}", user.Username, user.AcademicId);
                throw;
            }
        }

        public async Task UpdateAsync(User user, CancellationToken ct = default)
        {
            const string sql = @"
                UPDATE dbo.Users
                SET AcademicId = @AcademicId, Username = @Username, FullName = @FullName, Email = @Email, Role = @Role
                WHERE Id = @Id;";
            await using var conn = _factory.CreateApp();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", user.Id);
            cmd.Parameters.AddWithValue("@AcademicId", user.AcademicId);
            cmd.Parameters.AddWithValue("@Username", user.Username);
            cmd.Parameters.AddWithValue("@FullName", user.FullName);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@Role", user.Role);

            try
            {
                await conn.OpenAsync(ct);
                await cmd.ExecuteNonQueryAsync(ct);
                _logger.LogInformation("User updated successfully: {Username}, AcademicId: {AcademicId}", user.Username, user.AcademicId);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Failed to update user: {Username}, AcademicId: {AcademicId}", user.Username, user.AcademicId);
                throw;
            }
        }

        public async Task DeleteAsync(int userId, CancellationToken ct = default)
        {
            const string sql = @"DELETE FROM dbo.Users WHERE Id = @Id;";
            await using var conn = _factory.CreateApp();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", userId);
            await conn.OpenAsync(ct);
            await cmd.ExecuteNonQueryAsync(ct);
        }

        public async Task<User?> GetByIdAsync(int userId, CancellationToken ct = default)
        {
            const string sql = @"
                SELECT Id, AcademicId, Username, PasswordHash, Salt, FullName, Email, Role
                FROM dbo.Users
                WHERE Id = @Id;";
            await using var conn = _factory.CreateApp();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", userId);

            try
            {
                await conn.OpenAsync(ct);
                await using var rdr = await cmd.ExecuteReaderAsync(ct);
                if (!await rdr.ReadAsync(ct))
                {
                    return new User
                    {
                        Id = rdr.GetInt32(0),
                        AcademicId = rdr.GetString(1),
                        Username = rdr.GetString(2),
                        PasswordHash = rdr.GetString(3),
                        Salt = rdr.GetString(4),
                        FullName = rdr.GetString(5),
                        Email = rdr.GetString(6),
                        Role = UserRoleConverter.FromString(rdr.GetString(7)) // Convertir string a enum
                    };

                }
                return null;

            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Failed to get user by ID: {Id}", userId);
                throw;
            }

        }

        public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default)
        {
            const string sql = @"
                SELECT Id, AcademicId, Username, PasswordHash, Salt, FullName, Email, Role
                FROM dbo.Users
                WHERE Username = @Username;";
            await using var conn = _factory.CreateApp();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Username", username);

            try
            {
                await conn.OpenAsync(ct);
                await using var rdr = await cmd.ExecuteReaderAsync(ct);
                if (await rdr.ReadAsync(ct))
                {
                    return new User
                    {
                        Id = rdr.GetInt32(0),
                        AcademicId = rdr.GetString(1),
                        Username = rdr.GetString(2),
                        PasswordHash = rdr.GetString(3),
                        Salt = rdr.GetString(4),
                        FullName = rdr.GetString(5),
                        Email = rdr.GetString(6),
                        Role = UserRoleConverter.FromString(rdr.GetString(7)) // Convertir string a enum
                    };
                }
                return null;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Failed to get user by username: {Username}", username);
                throw;
            }
        }
    }
}