using BlazorApp.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace BlazorApp.Services;

public class EnrollmentService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationService _authService;

    public EnrollmentService(HttpClient httpClient, AuthenticationService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    // Obtener comisiones en las que el usuario está inscripto
    public async Task<List<int>?> GetEnrolledCommissionIdsAsync(int userId)
    {
        try
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"/api/Enrollments/user/{userId}/commissions"
            );

            Console.WriteLine($"🔍 Cargando comisiones inscriptas para usuario {userId}");

            var response = await _authService.SendAuthorizedAsync(request);

            Console.WriteLine($"📡 Respuesta comisiones: {response.StatusCode}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"📄 Datos recibidos: {content}");
                return JsonSerializer.Deserialize<List<int>>(content);
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 Error al cargar comisiones inscriptas: {ex.Message}");
            return null;
        }
    }
}