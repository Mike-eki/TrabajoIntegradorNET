using BlazorApp.Models;

namespace BlazorApp.Services;

public class UserProfileService
{
    private readonly AuthenticationService _authService;
    private readonly HttpClient _httpClient;

    // ✅ Almacenamos el perfil una vez cargado
    public UserProfile? CurrentProfile { get; private set; }

    public UserProfileService(AuthenticationService authService, HttpClient httpClient)
    {
        _authService = authService;
        _httpClient = httpClient;
    }

    // ✅ Método público para cargar (y cachear) el perfil
    public async Task<bool> LoadProfileAsync()
    {
        if (!_authService.IsAuthenticated)
        {
            Console.WriteLine("❌ No autenticado");
            CurrentProfile = null;
            return false;
        }

        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/Users/profile");
            var response = await _authService.SendAuthorizedAsync(request); // ✅ Reutiliza

            Console.WriteLine($"📡 Respuesta API: {response.StatusCode}");

            if (response.IsSuccessStatusCode)
            {
                CurrentProfile = await response.Content.ReadFromJsonAsync<UserProfile>();
                Console.WriteLine($"✅ Perfil cargado: {CurrentProfile?.FullName}");
                return true;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _authService.Logout();
                CurrentProfile = null;
                return false;
            }
            else
            {
                CurrentProfile = null;
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 Error: {ex.Message}");
            CurrentProfile = null;
            return false;
        }
    }

    // ✅ Opcional: método para forzar recarga
    public void ClearProfile() => CurrentProfile = null;
}