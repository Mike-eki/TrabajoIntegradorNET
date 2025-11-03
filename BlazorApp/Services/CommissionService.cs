using BlazorApp.Models.Commissions.razor;
using System.Net.Http.Json;
using System.Text.Json;

namespace BlazorApp.Services;

public class CommissionService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationService _authService;
    private string enrollmentError;

    public CommissionService(HttpClient httpClient, AuthenticationService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    // Obtener TODAS las comisiones activas (sin filtrar por materia)
    public async Task<List<CommissionWithProfessorDto>?> GetActiveCommissionsAsync()
    {

        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/Commissions?status=Activo");
            var response = await _authService.SendAuthorizedAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<CommissionWithProfessorDto>>()
                       ?? new List<CommissionWithProfessorDto>();
            }

            return new List<CommissionWithProfessorDto>();
        }
        catch
        {
            return new List<CommissionWithProfessorDto>();
        }
    }

    // Auto-inscripción
    public async Task<bool> SelfEnrollAsync(int studentId, int commissionId)
    {
        try
        {
            var requestDto = new SelfEnrollmentRequestDto
            {
                StudentId = studentId,
                CommissionId = commissionId
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/Enrollments/self");
            request.Content = JsonContent.Create(requestDto);

            // ✅ Depurar contenido
            var contentDebug = await request.Content.ReadAsStringAsync();
            Console.WriteLine($"📤 Solicitud: {contentDebug}");

            Console.WriteLine($"🔍 Enviando solicitud de inscripción: StudentId={studentId}, CommissionId={commissionId}");

            var response = await _authService.SendAuthorizedAsync(request);

            Console.WriteLine($"📡 Respuesta API: {response.StatusCode}");
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"❌ Error de API: {errorContent}");

                // ✅ Parsear el mensaje específico de la API
                try
                {
                    var errorResponse = JsonSerializer.Deserialize<ApiErrorResponse>(errorContent);
                    enrollmentError = errorResponse?.Message ?? "Error desconocido";
                }
                catch
                {
                    enrollmentError = "Error al procesar respuesta del servidor";
                }


                // ✅ Analizar errores específicos
                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    Console.WriteLine("⚠️ Error 403: Usuario no autorizado para esta acción");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    Console.WriteLine("⚠️ Error 400: Solicitud mal formada");
                }

                return false;
            }

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 Excepción al inscribirse: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
            return false;
        }
    }

    // ✅ Clase auxiliar
    private class ApiErrorResponse
    {
        public string? Message { get; set; }
    }
}