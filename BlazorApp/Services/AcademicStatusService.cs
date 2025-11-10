using BlazorApp.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace BlazorApp.Services;

public class AcademicStatusService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationService _authService;

    public AcademicStatusService(HttpClient httpClient, AuthenticationService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    public async Task<(List<AcademicStatusDto>? data, string? error)> GetAcademicStatusByCareerAsync(int userId, int careerId)
    {
        try
        {
            Console.WriteLine($"🔍 Iniciando carga de estado académico para usuario {userId}, carrera {careerId}");

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"/api/Enrollments/academicstatus/user/{userId}/career/{careerId}"
            );

            var response = await _authService.SendAuthorizedAsync(request);

            Console.WriteLine($"📡 Respuesta API: {response.StatusCode}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"📄 Datos recibidos: {content}");

                try
                {
                    var result = JsonSerializer.Deserialize<List<AcademicStatusDto>>(content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    Console.WriteLine($"✅ Deserialización exitosa. Materias cargadas: {result?.Count ?? 0}");
                    return (result, null);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error al deserializar: {ex.Message}");
                    Console.WriteLine($"Contenido recibido: {content}");
                    return (null, $"Error al procesar los datos: {ex.Message}");
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"❌ Contenido de error: {errorContent}");

                // Intentar parsear mensaje de error
                try
                {
                    var errorResponse = JsonSerializer.Deserialize<ApiErrorResponse>(errorContent);
                    return (null, errorResponse?.Message ?? $"Error del servidor: {response.StatusCode}");
                }
                catch
                {
                    return (null, $"Error del servidor: {response.StatusCode}. {errorContent}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 Excepción en GetAcademicStatusByCareerAsync: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
            return (null, $"Error de conexión: {ex.Message}");
        }
    }
}

// Modelo en Blazor (debe coincidir con el DTO de la API)
public class AcademicStatusDto
{
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public decimal? FinalGrade { get; set; }
}

public class ApiErrorResponse
{
    public string? Message { get; set; }
}
