using Models.DTOs;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace WinFormsAdmin.Services
{
    public class ApiClient
    {
        // ✅ CORREGIDO: Hacer HttpClient estático para compartirlo
        private static readonly HttpClient _httpClient;
        private static string? _accessToken;
        private static string? _refreshToken;

        private const string BaseUrl = "https://localhost:7114";

        // ✅ CORREGIDO: Usar constructor estático para inicializar HttpClient UNA sola vez
        static ApiClient()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        }

        // El constructor de instancia ahora está vacío
        public ApiClient()
        {
        }

        // Método para login (Tu lógica aquí estaba bien)
        public async Task<(AuthResponse? Response, string? ErrorMessage)> LoginAsync(string username, string password)
        {
            try
            {
                var loginData = new { username, password };
                var response = await _httpClient.PostAsJsonAsync("/api/Users/validate", loginData);

                if (response.IsSuccessStatusCode)
                {
                    var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();

                    if (authResponse != null && authResponse.IsValid)
                    {
                        _accessToken = authResponse.AccessToken;
                        _refreshToken = authResponse.RefreshToken;
                        _httpClient.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", _accessToken);
                        return (authResponse, null);
                    }
                    else
                    {
                        return (null, "Respuesta del servidor inválida.");
                    }
                }
                else
                {
                    string errorMessage = "Credenciales inválidas.";
                    // ... (Tu manejo de errores de login está bien) ...
                    var errorText = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(errorText))
                    {
                        errorMessage = errorText; // Simplificado para el ejemplo
                    }
                    return (null, errorMessage);
                }
            }
            catch (HttpRequestException ex)
            {
                return (null, $"No se pudo conectar con el servidor: {ex.Message}");
            }
            catch (Exception ex)
            {
                return (null, $"Error inesperado: {ex.Message}");
            }
        }

        // RefreshTokenAsync (Tu lógica aquí estaba bien)
        public async Task<bool> RefreshTokenAsync()
        {
            if (string.IsNullOrEmpty(_accessToken) || string.IsNullOrEmpty(_refreshToken))
                return false;

            var request = new
            {
                AccessToken = _accessToken,
                RefreshToken = _refreshToken
            };

            // Usamos _httpClient (que ahora es estático)
            var response = await _httpClient.PostAsJsonAsync("/api/Users/refresh", request);

            if (!response.IsSuccessStatusCode)
                return false;

            var newTokens = await response.Content.ReadFromJsonAsync<AuthResponse>();
            if (newTokens == null || string.IsNullOrEmpty(newTokens.AccessToken))
                return false;

            _accessToken = newTokens.AccessToken;
            _refreshToken = newTokens.RefreshToken;
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _accessToken);

            return true;
        }


        // Métodos genéricos para CRUD

        // GetAsync (Tu lógica aquí estaba bien)
        public async Task<T?> GetAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (await RefreshTokenAsync())
                {
                    response = await _httpClient.GetAsync(endpoint);
                }
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        // GetListAsync (Tu lógica aquí estaba bien)
        public async Task<List<T>?> GetListAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (await RefreshTokenAsync())
                {
                    response = await _httpClient.GetAsync(endpoint);
                }
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<T>>();
        }

        public async Task<T?> PostAsync<T>(string endpoint, object data)
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, data);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (await RefreshTokenAsync())
                {
                    // ✅ CORREGIDO: Reintentar con POST y 'data'
                    response = await _httpClient.PostAsJsonAsync(endpoint, data);
                }
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        // ✅ CORREGIDO: Devuelve Task (void) y usa EnsureSuccessStatusCode
        public async Task PutAsync(string endpoint, object data)
        {
            var response = await _httpClient.PutAsJsonAsync(endpoint, data);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (await RefreshTokenAsync())
                {
                    // ✅ CORREGIDO: Reintentar con PUT y 'data'
                    response = await _httpClient.PutAsJsonAsync(endpoint, data);
                }
            }

            // ✅ CORREGIDO: Lanzar excepción si falla
            response.EnsureSuccessStatusCode();
        }

        // ✅ CORREGIDO: Devuelve Task (void) y usa EnsureSuccessStatusCode
        public async Task DeleteAsync(string endpoint)
        {
            var response = await _httpClient.DeleteAsync(endpoint);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (await RefreshTokenAsync())
                {
                    // ✅ CORREGIDO: Reintentar con DELETE
                    response = await _httpClient.DeleteAsync(endpoint);
                }
            }

            // ✅ CORREGIDO: Lanzar excepción si falla
            response.EnsureSuccessStatusCode();
        }

        public bool IsAuthenticated() => !string.IsNullOrEmpty(_accessToken);

        // LogoutAsync (Tu lógica aquí estaba bien)
        public async Task LogoutAsync()
        {
            var accessToken = _accessToken;
            using var request = new HttpRequestMessage(HttpMethod.Post, "/api/Users/logout");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            await _httpClient.SendAsync(request);
            _accessToken = null;
            _refreshToken = null; // También limpiar el refresh token
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}