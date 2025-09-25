using Repositories;
using Repositories.Interfaces;
using Repositories.AdoNet;
using Repositories.EF;
using Services;
using Services.Interfaces;
using System;


var builder = WebApplication.CreateBuilder(args);

// Configurar cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registrar repositorios según la entidad
builder.Services.AddScoped<IUserRepository>(provider =>
    new AdoNetUserRepository(connectionString));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<ICommissionRepository, EFCommissionRepository>();
builder.Services.AddScoped<ICourseRepository, EFCourseRepository>();

// Add services to the container.
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAcademicService, AcademicService>();
builder.Services.AddScoped<IAcademicRepository, AcademicRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
