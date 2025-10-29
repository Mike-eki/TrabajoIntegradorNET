using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ADO.NET
{
    public sealed class DatabaseInitializer
    {
        private readonly DbConnectionFactory _factory;
        private readonly string _databaseName;
        private readonly ILogger<DatabaseInitializer> _logger;
        private const int MaxRetries = 3;
        private static readonly TimeSpan RetryDelay = TimeSpan.FromSeconds(2);

        public DatabaseInitializer(DbConnectionFactory factory, string databaseName, ILogger<DatabaseInitializer> logger)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _databaseName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task EnsureCreatedAsync(CancellationToken ct = default)
        {
            _logger.LogInformation("Starting database initialization for {DatabaseName}.", _databaseName);
            await CreateDatabaseIfNotExistsAsync(ct);
            await CreateUsersTableIfNotExistsAsync(ct);
            await CreateUserCareerTableIfNotExistsAsync(ct);
            await CreateRefreshTokensTableIfNotExistsAsync(ct);
            await CreateRevokedAccessTokensTableIfNotExistsAsync(ct);
            await CreateValidateUserProcIfNotExistsAsync(ct);
            _logger.LogInformation("Database initialization completed for {DatabaseName}.", _databaseName);
        }

        private async Task CreateDatabaseIfNotExistsAsync(CancellationToken ct)
        {
            var sql = $@"IF DB_ID(N'{EscapeSqlIdentifier(_databaseName)}') IS NULL CREATE DATABASE [{_databaseName}];";
            _logger.LogDebug("Executing SQL to create database if not exists: {Sql}", sql);
            await ExecuteWithRetryAsync(_factory.CreateMaster(), sql, ct);
        }

        private async Task CreateUsersTableIfNotExistsAsync(CancellationToken ct)
        {
            // ✨ ACTUALIZADO CON NUEVAS COLUMNAS Y CONSTRAINTS ✨
            const string sql = @"
        IF NOT EXISTS (SELECT 1 FROM sys.tables t JOIN sys.schemas s ON t.schema_id = s.schema_id
                       WHERE t.name = 'Users' AND s.name = 'dbo')
        BEGIN
            CREATE TABLE dbo.Users
            (
                Id INT IDENTITY(1,1) PRIMARY KEY,
                Username NVARCHAR(50) NOT NULL CONSTRAINT UQ_Users_Username UNIQUE,
                Legajo NVARCHAR(50) NOT NULL CONSTRAINT UQ_Users_Legajo UNIQUE,
                Email NVARCHAR(100) NOT NULL CONSTRAINT UQ_Users_Email UNIQUE,
                Fullname NVARCHAR(100) NOT NULL,
                PasswordHash NVARCHAR(256) NOT NULL,
                Salt NVARCHAR(44) NOT NULL,
                Role NVARCHAR(20) NOT NULL DEFAULT 'Student' 
                    CONSTRAINT CHK_Users_Role CHECK (Role IN ('Admin', 'Student', 'Professor'))
            );
        END
        ELSE
        BEGIN
            -- ✨ SECCIÓN ELSE ACTUALIZADA PARA AÑADIR COLUMNAS SI FALTAN ✨
            -- Añadir Role (ya lo tenías, se mantiene)
            IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.Users') AND name = 'Role')
            BEGIN
                ALTER TABLE dbo.Users
                ADD Role NVARCHAR(20) NOT NULL DEFAULT 'Student' 
                    CONSTRAINT CHK_Users_Role CHECK (Role IN ('Admin', 'Student', 'Professor'));
            END
            -- Añadir Legajo
            IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.Users') AND name = 'Legajo')
            BEGIN
                ALTER TABLE dbo.Users
                ADD Legajo NVARCHAR(50) NOT NULL DEFAULT 'N/A';
                ALTER TABLE dbo.Users ADD CONSTRAINT UQ_Users_Legajo UNIQUE (Legajo);
            END
            -- Añadir Email
            IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.Users') AND name = 'Email')
            BEGIN
                ALTER TABLE dbo.Users
                ADD Email NVARCHAR(100) NOT NULL DEFAULT 'no-email@domain.com';
                ALTER TABLE dbo.Users ADD CONSTRAINT UQ_Users_Email UNIQUE (Email);
            END
            -- Añadir Fullname
            IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.Users') AND name = 'Fullname')
            BEGIN
                ALTER TABLE dbo.Users
                ADD Fullname NVARCHAR(100) NOT NULL DEFAULT 'No Name';
            END
        END";
            _logger.LogDebug("Executing SQL to create or update Users table.");
            await ExecuteAsync(_factory.CreateApp(), sql, ct);

        }

        private async Task CreateRefreshTokensTableIfNotExistsAsync(CancellationToken ct)
        {
            const string sql = @"
                IF NOT EXISTS (SELECT 1 FROM sys.tables t JOIN sys.schemas s ON t.schema_id = s.schema_id
                               WHERE t.name = 'RefreshTokens' AND s.name = 'dbo')
                BEGIN
                    CREATE TABLE dbo.RefreshTokens
                    (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        UserId INT NOT NULL CONSTRAINT FK_RefreshTokens_Users FOREIGN KEY REFERENCES dbo.Users(Id),
                        Token NVARCHAR(256) NOT NULL,
                        ExpiresAt DATETIME2 NOT NULL,
                        IsRevoked BIT NOT NULL DEFAULT 0,
                        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
                        CONSTRAINT UQ_RefreshTokens_Token UNIQUE (Token)
                    );
                END";
            _logger.LogDebug("Executing SQL to create RefreshTokens table.");
            await ExecuteAsync(_factory.CreateApp(), sql, ct);
        }

        private async Task CreateRevokedAccessTokensTableIfNotExistsAsync(CancellationToken ct)
        {
            const string sql = @"
                IF NOT EXISTS (SELECT 1 FROM sys.tables t JOIN sys.schemas s ON t.schema_id = s.schema_id
                               WHERE t.name = 'RevokedAccessTokens' AND s.name = 'dbo')
                BEGIN
                    CREATE TABLE dbo.RevokedAccessTokens
                    (
                        Token NVARCHAR(1024) PRIMARY KEY,
                        RevokedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
                    );
                END";
            _logger.LogInformation("Executing SQL to create RevokedAccessTokens table.");
            await ExecuteAsync(_factory.CreateApp(), sql, ct);
        }

        private async Task CreateUserCareerTableIfNotExistsAsync(CancellationToken ct)
        {
            const string sql = @"
    IF NOT EXISTS (
        SELECT 1 
        FROM sys.tables t 
        JOIN sys.schemas s ON t.schema_id = s.schema_id
        WHERE t.name = 'UserCareer' AND s.name = 'dbo'
    )
    BEGIN
        CREATE TABLE dbo.UserCareer
        (
            UserId INT NOT NULL,
            CareerId INT NOT NULL,
            CONSTRAINT PK_UserCareer PRIMARY KEY (UserId, CareerId),
            CONSTRAINT FK_UserCareer_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(Id),
            CONSTRAINT FK_UserCareer_Careers FOREIGN KEY (CareerId) REFERENCES dbo.Careers(Id)
        );
    END
    ELSE
    BEGIN
        -- Verificar que las columnas principales existan
        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.UserCareer') AND name = 'UserId')
        BEGIN
            ALTER TABLE dbo.UserCareer ADD UserId INT NOT NULL DEFAULT 0;
        END

        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.UserCareer') AND name = 'CareerId')
        BEGIN
            ALTER TABLE dbo.UserCareer ADD CareerId INT NOT NULL DEFAULT 0;
        END

        -- Verificar PK
        IF NOT EXISTS (
            SELECT 1 FROM sys.key_constraints 
            WHERE parent_object_id = OBJECT_ID(N'dbo.UserCareer') 
              AND type = 'PK' 
              AND name = 'PK_UserCareer'
        )
        BEGIN
            ALTER TABLE dbo.UserCareer ADD CONSTRAINT PK_UserCareer PRIMARY KEY (UserId, CareerId);
        END

        -- Verificar FK a Users
        IF NOT EXISTS (
            SELECT 1 FROM sys.foreign_keys 
            WHERE parent_object_id = OBJECT_ID(N'dbo.UserCareer') 
              AND name = 'FK_UserCareer_Users'
        )
        BEGIN
            ALTER TABLE dbo.UserCareer 
            ADD CONSTRAINT FK_UserCareer_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(Id);
        END

        -- Verificar FK a Careers
        IF NOT EXISTS (
            SELECT 1 FROM sys.foreign_keys 
            WHERE parent_object_id = OBJECT_ID(N'dbo.UserCareer') 
              AND name = 'FK_UserCareer_Careers'
        )
        BEGIN
            ALTER TABLE dbo.UserCareer 
            ADD CONSTRAINT FK_UserCareer_Careers FOREIGN KEY (CareerId) REFERENCES dbo.Careers(Id);
        END
    END";

            _logger.LogInformation("Ensuring UserCareer table exists.");
            await ExecuteAsync(_factory.CreateApp(), sql, ct);
        }


        private async Task CreateValidateUserProcIfNotExistsAsync(CancellationToken ct)
        {
            const string checkProcSql = @"
                SELECT COUNT(1)
                FROM sys.objects
                WHERE object_id = OBJECT_ID(N'dbo.ValidateUser') AND type IN (N'P', N'PC')";

            _logger.LogDebug("Checking if ValidateUser procedure exists.");
            await using var conn = _factory.CreateApp();
            await using var checkCmd = new SqlCommand(checkProcSql, conn) { CommandType = CommandType.Text };
            await conn.OpenAsync(ct);
            var exists = (int)await checkCmd.ExecuteScalarAsync(ct) > 0;

            if (!exists)
            {
                const string createProcSql = @"
                CREATE PROCEDURE dbo.ValidateUser
                    @Username NVARCHAR(50),
                    @PasswordHash NVARCHAR(256) OUTPUT,
                    @Salt NVARCHAR(44) OUTPUT,
                    @Role NVARCHAR(20) OUTPUT
                AS
                BEGIN
                    SET NOCOUNT ON;
                    SELECT @PasswordHash = PasswordHash, @Salt = Salt, @Role = Role 
                    FROM dbo.Users 
                    WHERE Username = @Username;
                END";
                _logger.LogDebug("Creating ValidateUser procedure.");
                await ExecuteAsync(_factory.CreateApp(), createProcSql, ct);
            }
            else
            {
                _logger.LogDebug("ValidateUser procedure already exists.");
            }
        }

        private static async Task ExecuteAsync(SqlConnection conn, string sql, CancellationToken ct)
        {
            await using var cmd = new SqlCommand(sql, conn) { CommandType = CommandType.Text, CommandTimeout = 30 };
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync(ct);
            await cmd.ExecuteNonQueryAsync(ct);
        }

        private static async Task ExecuteWithRetryAsync(SqlConnection conn, string sql, CancellationToken ct)
        {
            Exception? last = null;
            for (int i = 0; i < MaxRetries; i++)
            {
                try
                {
                    await ExecuteAsync(conn, sql, ct);
                    return;
                }
                catch (SqlException ex) when (IsTransient(ex) && i < MaxRetries - 1)
                {
                    last = ex;
                    await Task.Delay(RetryDelay, ct);
                }
            }
            throw last ?? new Exception("Failed to execute SQL after retries.");
        }

        private static bool IsTransient(SqlException ex)
        {
            foreach (SqlError err in ex.Errors)
            {
                if (err.Number is 4060 or 40197 or 40501 or 49918) return true;
            }
            return false;
        }

        private static string EscapeSqlIdentifier(string identifier) =>
            identifier?.Replace("]", "]]") ?? throw new ArgumentNullException(nameof(identifier));
    }
}