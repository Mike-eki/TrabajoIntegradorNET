using ADO.NET;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging; // Necesario para ILogger
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Swagger with JWT support
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "MyApp API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by a space and the JWT token."
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Add logging
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.SetMinimumLevel(LogLevel.Information); // Cambia a LogLevel.Debug para más detalles
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not found in configuration."))),
        ClockSkew = TimeSpan.FromSeconds(30) // Tolerancia de 30 segundos para desincronización
    };
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Token validated. SecurityToken Type: {Type}, Id: {Id}",
                context.SecurityToken?.GetType().FullName, context.SecurityToken?.Id);

            var rawToken = context.Request.Headers.Authorization.ToString().Replace("Bearer ", "").Trim();
            if (string.IsNullOrEmpty(rawToken))
            {
                logger.LogWarning("No token found in Authorization header.");
                context.Fail("No token provided");
                return;
            }

            var revokedAccessTokenRepo = context.HttpContext.RequestServices.GetRequiredService<IRevokedAccessTokenRepository>();
            if (await revokedAccessTokenRepo.IsRevokedAsync(rawToken))
            {
                logger.LogWarning("Access token revoked: {Token}", rawToken);
                context.Fail("Access token has been revoked");
            }
        },
        OnAuthenticationFailed = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogError(context.Exception, "JWT Authentication failed: {Message}. Token: {Token}. Headers: {Headers}",
                context.Exception.Message,
                context.Request.Headers["Authorization"],
                string.Join(", ", context.Request.Headers.Select(h => $"{h.Key}: {h.Value}")));
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogWarning("JWT Challenge: {ErrorDescription}", context.ErrorDescription);
            return Task.CompletedTask;
        }
    };
});

// Validate and retrieve connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found in configuration.");

// Register ADO.NET services
builder.Services.AddSingleton<DbConnectionFactory>(sp =>
    new DbConnectionFactory(connectionString));
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddSingleton<IRevokedAccessTokenRepository, RevokedAccessTokenRepository>();
builder.Services.AddSingleton<UserService>(sp =>
    new UserService(
        sp.GetRequiredService<IUserRepository>(),
        sp.GetRequiredService<IRefreshTokenRepository>(),
        sp.GetRequiredService<IRevokedAccessTokenRepository>(),
        sp.GetRequiredService<ILogger<UserService>>(),
        sp.GetRequiredService<IConfiguration>()
    ));
builder.Services.AddSingleton<DatabaseInitializer>(sp =>
    new DatabaseInitializer(
        sp.GetRequiredService<DbConnectionFactory>(),
        new SqlConnectionStringBuilder(connectionString).InitialCatalog,
        sp.GetRequiredService<ILogger<DatabaseInitializer>>()
    ));

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware temporal para depurar headers
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    logger.LogDebug("Incoming Request Path: {Path}", context.Request.Path);
    logger.LogDebug("Incoming Request Headers: {Headers}", context.Request.Headers);
    await next();
});

app.UseHttpsRedirection();
app.UseCors("AllowAll");    
app.UseAuthentication(); // Add JWT authentication
app.UseAuthorization();
app.MapControllers();

// Initialize database with logging
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
        await initializer.EnsureCreatedAsync();
        logger.LogInformation("Database initialized successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Failed to initialize database.");
        throw;
    }
}

// Revoke all tokens on shutdown
app.Lifetime.ApplicationStopping.Register(async () =>
{
    using var scope = app.Services.CreateScope();
    var refreshTokenRepo = scope.ServiceProvider.GetRequiredService<IRefreshTokenRepository>();
    var revokedAccessTokenRepo = scope.ServiceProvider.GetRequiredService<IRevokedAccessTokenRepository>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Application stopping. Revoking all active tokens.");

    const string sql = @"UPDATE dbo.RefreshTokens SET IsRevoked = 1 WHERE IsRevoked = 0;
                         DELETE FROM dbo.RevokedAccessTokens;";
    await using var conn = new SqlConnection(connectionString);
    await using var cmd = new SqlCommand(sql, conn);
    await conn.OpenAsync();
    await cmd.ExecuteNonQueryAsync();
    logger.LogInformation("All tokens revoked.");
});

await app.RunAsync();