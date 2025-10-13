using Microsoft.Data.SqlClient;
using Models.Entities;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace ADO.NET
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly DbConnectionFactory _factory;

        public UserRepository(DbConnectionFactory factory)
        {
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
            // ✨ SQL ACTUALIZADO ✨
            const string sql = @"INSERT INTO dbo.Users (Username, Legajo, Email, Fullname, PasswordHash, Salt, Role) 
                        OUTPUT INSERTED.Id 
                        VALUES (@Username, @Legajo, @Email, @Fullname, @PasswordHash, @Salt, @Role);";
            await using var conn = _factory.CreateApp();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Username", user.Username);
            // ✨ AÑADIR NUEVOS PARÁMETROS ✨
            cmd.Parameters.AddWithValue("@Legajo", user.Legajo);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@Fullname", user.Fullname);
            cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            cmd.Parameters.AddWithValue("@Salt", user.Salt);
            cmd.Parameters.AddWithValue("@Role", UserRoleConverter.ToString(user.Role));
            await conn.OpenAsync(ct);
            user.Id = (int)await cmd.ExecuteScalarAsync(ct);
        }

        public async Task UpdateAsync(User user, CancellationToken ct = default)
        {
            // ✨ SQL ACTUALIZADO ✨
            const string sql = @"UPDATE dbo.Users 
                        SET Username = @Username, Legajo = @Legajo, Email = @Email, Fullname = @Fullname, 
                            PasswordHash = @PasswordHash, Salt = @Salt, Role = @Role 
                        WHERE Id = @Id;";
            await using var conn = _factory.CreateApp();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", user.Id);
            cmd.Parameters.AddWithValue("@Username", user.Username);
            // ✨ AÑADIR NUEVOS PARÁMETROS ✨
            cmd.Parameters.AddWithValue("@Legajo", user.Legajo);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@Fullname", user.Fullname);
            cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            cmd.Parameters.AddWithValue("@Salt", user.Salt);
            cmd.Parameters.AddWithValue("@Role", UserRoleConverter.ToString(user.Role));
            await conn.OpenAsync(ct);
            await cmd.ExecuteNonQueryAsync(ct);
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
            // ✨ SQL ACTUALIZADO ✨
            const string sql = @"SELECT Id, Username, Legajo, Email, Fullname, PasswordHash, Salt, Role 
                        FROM dbo.Users 
                        WHERE Id = @Id;";
            await using var conn = _factory.CreateApp();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", userId);
            await conn.OpenAsync(ct);
            await using var rdr = await cmd.ExecuteReaderAsync(ct);
            if (!await rdr.ReadAsync(ct)) return null;

            // ✨ MAPEO ACTUALIZADO ✨
            return new User
            {
                Id = rdr.GetInt32(0),
                Username = rdr.GetString(1),
                Legajo = rdr.GetString(2),     // Nuevo
                Email = rdr.GetString(3),      // Nuevo
                Fullname = rdr.GetString(4),   // Nuevo
                PasswordHash = rdr.GetString(5),
                Salt = rdr.GetString(6),
                Role = UserRoleConverter.FromString(rdr.GetString(7))
            };
        }

        public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default)
        {
            // ✨ SQL ACTUALIZADO ✨
            const string sql = @"SELECT Id, Username, Legajo, Email, Fullname, PasswordHash, Salt, Role 
                        FROM dbo.Users 
                        WHERE Username = @Username;";
            await using var conn = _factory.CreateApp();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Username", username);
            await conn.OpenAsync(ct);
            await using var rdr = await cmd.ExecuteReaderAsync(ct);
            if (!await rdr.ReadAsync(ct)) return null;

            // ✨ MAPEO ACTUALIZADO ✨
            return new User
            {
                Id = rdr.GetInt32(0),
                Username = rdr.GetString(1),
                Legajo = rdr.GetString(2),     // Nuevo
                Email = rdr.GetString(3),      // Nuevo
                Fullname = rdr.GetString(4),   // Nuevo
                PasswordHash = rdr.GetString(5),
                Salt = rdr.GetString(6),
                Role = UserRoleConverter.FromString(rdr.GetString(7))
            };
        }
    }
}