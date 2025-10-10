using ADO.NET;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging; // Necesario para ILogger
using Microsoft.IdentityModel.Tokens;
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
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not found in configuration.")))
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogError(context.Exception, "JWT Authentication failed: {Message}", context.Exception.Message);
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
builder.Services.AddSingleton<UserService>(sp =>
    new UserService(
        sp.GetRequiredService<IUserRepository>(),
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

await app.RunAsync();