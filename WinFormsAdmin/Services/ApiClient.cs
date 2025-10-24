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
        private readonly HttpClient _httpClient;
        private static string? _accessToken;
        private static string? _refreshToken;

        // URL base de tu WebAPI
        private const string BaseUrl = "https://localhost:7114"; // ⚠️ Ajustar al puerto de tu API

        public ApiClient()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        }

        // Método para login
        public async Task<(AuthResponse? Response, string? ErrorMessage)> LoginAsync(string username, string password)
        {
            try
            {
                var loginData = new { username, password };
                var response = await _httpClient.PostAsJsonAsync("/api/Users/validate", loginData);

                if (response.IsSuccessStatusCode)
                {
                    // ✅ Login exitoso
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
                    // ❌ Error de autenticación
                    string errorMessage = "Credenciales inválidas.";

                    // Intentar leer el cuerpo de la respuesta
                    var contentType = response.Content.Headers.ContentType?.MediaType;

                    if (contentType == "application/json")
                    {
                        try
                        {
                            // Si la API devuelve JSON con estructura { "message": "..." }
                            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                            if (errorResponse != null && !string.IsNullOrEmpty(errorResponse.Message))
                            {
                                errorMessage = errorResponse.Message;
                            }
                        }
                        catch
                        {
                            // Si falla la deserialización, intentar leer como string
                            var errorText = await response.Content.ReadAsStringAsync();
                            if (!string.IsNullOrEmpty(errorText))
                            {
                                errorMessage = errorText;
                            }
                        }
                    }
                    else
                    {
                        // Si no es JSON, leer como texto plano
                        var errorText = await response.Content.ReadAsStringAsync();
                        if (!string.IsNullOrEmpty(errorText))
                        {
                            errorMessage = errorText;
                        }
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

        public async Task<bool> RefreshTokenAsync()
        {
            if (string.IsNullOrEmpty(_accessToken) || string.IsNullOrEmpty(_refreshToken))
                return false;

            var request = new
            {
                AccessToken = _accessToken,
                RefreshToken = _refreshToken
            };

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
        public async Task<T?> GetAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);

            // Si el AccessToken es revoked, refrescarlo y enviar de nuevo
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (await RefreshTokenAsync())
                {
                    // Intentar de nuevo con el token renovado
                    response = await _httpClient.GetAsync(endpoint);
                }
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<List<T>?> GetListAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);

            // Si el AccessToken es revoked, refrescarlo y enviar de nuevo
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (await RefreshTokenAsync())
                {
                    // Intentar de nuevo con el token renovado
                    response = await _httpClient.GetAsync(endpoint);
                }
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<T>>();
        }

        public async Task<T?> PostAsync<T>(string endpoint, object data)
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, data);

            // Si el AccessToken es revoked, refrescarlo y enviar de nuevo
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (await RefreshTokenAsync())
                {
                    // Intentar de nuevo con el token renovado
                    response = await _httpClient.GetAsync(endpoint);
                }
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<bool> PutAsync(string endpoint, object data)
        {
            var response = await _httpClient.PutAsJsonAsync(endpoint, data);

            // Si el AccessToken es revoked, refrescarlo y enviar de nuevo
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (await RefreshTokenAsync())
                {
                    // Intentar de nuevo con el token renovado
                    response = await _httpClient.GetAsync(endpoint);
                }
            }

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(string endpoint)
        {
            var response = await _httpClient.DeleteAsync(endpoint);

            // Si el AccessToken es revoked, refrescarlo y enviar de nuevo
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (await RefreshTokenAsync())
                {
                    // Intentar de nuevo con el token renovado
                    response = await _httpClient.GetAsync(endpoint);
                }
            }

            return response.IsSuccessStatusCode;
        }

        public bool IsAuthenticated() => !string.IsNullOrEmpty(_accessToken);

        public async Task LogoutAsync()
        {
            var accessToken = _accessToken;
            using var request = new HttpRequestMessage(HttpMethod.Post, "/api/Users/logout");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            await _httpClient.SendAsync(request);
            _accessToken = null;
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }

}