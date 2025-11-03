using BlazorApp.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace BlazorApp.Services;

public class ProfessorService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationService _authService;

    public ProfessorService(HttpClient httpClient, AuthenticationService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    // HU08: Obtener comisiones sin asignar
    public async Task<List<UnassignedCommissionDto>?> GetUnassignedCommissionsAsync()
    {
        try
        {
            Console.WriteLine("🔍 Obteniendo comisiones sin asignar...");
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/Commissions/unassigned");
            var response = await _authService.SendAuthorizedAsync(request);

            Console.WriteLine($"📡 Respuesta comisiones sin asignar: {response.StatusCode}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"📄 Datos recibidos: {content}");
                return JsonSerializer.Deserialize<List<UnassignedCommissionDto>>(content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 Error al obtener comisiones sin asignar: {ex.Message}");
            return null;
        }
    }

    // HU08: Asignar profesor a comisión
    public async Task<bool> AssignProfessorToCommissionAsync(int commissionId)
    {
        try
        {
            Console.WriteLine($"🔍 Asignando profesor a comisión {commissionId}...");
            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/Commissions/{commissionId}/assign-professor");
            var response = await _authService.SendAuthorizedAsync(request);

            Console.WriteLine($"📡 Respuesta asignación: {response.StatusCode}");
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"❌ Error de API: {errorContent}");
            }
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 Error al asignar profesor: {ex.Message}");
            return false;
        }
    }

    // HU09: Obtener estudiantes en mis comisiones
    public async Task<List<ProfessorCommissionStudentsDto>?> GetMyCommissionsStudentsAsync()
    {
        try
        {
            Console.WriteLine("🔍 Obteniendo estudiantes en mis comisiones...");
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/Enrollments/my-commissions/students");
            var response = await _authService.SendAuthorizedAsync(request);

            Console.WriteLine($"📡 Respuesta estudiantes: {response.StatusCode}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"📄 Datos recibidos: {content}");
                return JsonSerializer.Deserialize<List<ProfessorCommissionStudentsDto>>(content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 Error al obtener estudiantes: {ex.Message}");
            return null;
        }
    }

    // HU10: Asignar nota final
    public async Task<bool> SetFinalGradeAsync(int enrollmentId, int finalGrade)
    {
        try
        {
            Console.WriteLine($"🔍 Asignando nota {finalGrade} a inscripción {enrollmentId}...");
            var requestDto = new { FinalGrade = finalGrade };
            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/Enrollments/final-grade/{enrollmentId}");
            request.Content = JsonContent.Create(requestDto);

            var response = await _authService.SendAuthorizedAsync(request);

            Console.WriteLine($"📡 Respuesta asignación de nota: {response.StatusCode}");
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"❌ Error de API: {errorContent}");
            }
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 Error al asignar nota: {ex.Message}");
            return false;
        }
    }
}

// DTOs para la API
public class UnassignedCommissionDto
{
    public int Id { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public int CycleYear { get; set; }
    public string DayOfWeek { get; set; } = string.Empty;
    public string StartTime { get; set; } = string.Empty;
    public string EndTime { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public int EnrolledCount { get; set; }
    public int AvailableSeats { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool HasProfessor => false; // Solo para comisiones sin asignar
}

public class ProfessorCommissionStudentsDto
{
    public int CommissionId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public int CycleYear { get; set; }
    public string DayOfWeek { get; set; } = string.Empty;
    public string StartTime { get; set; } = string.Empty;
    public string EndTime { get; set; } = string.Empty;
    public List<StudentInCommissionDto> Students { get; set; } = new();
}

public class StudentInCommissionDto
{
    public int EnrollmentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string EnrollmentNumber { get; set; } = string.Empty;
    public int? FinalGrade { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ErrorMessage { get; set; } = string.Empty;
}