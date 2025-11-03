using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using BlazorApp.Models;

namespace BlazorApp.Services;

public class AuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly JwtTokenParserService _jwtParser;
    private LoginResponse? _authData;
    public int UserId { get; private set; }

    public bool IsAuthenticated => !string.IsNullOrEmpty(_authData?.AccessToken) && _authData.IsValid;
    public string? AccessToken => _authData?.AccessToken;
    public string? RefreshToken => _authData?.RefreshToken;
    public string? Role => _authData?.Role;
    public string? Username { get; private set; } // Opcional: si tu API no devuelve username, podrías guardarlo tú

    public AuthenticationService(HttpClient httpClient, JwtTokenParserService jwtParser)
    {
        _httpClient = httpClient;
        _jwtParser = jwtParser;
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        try
        {
            var loginRequest = new { Username = username, Password = password };
            var response = await _httpClient.PostAsJsonAsync("/api/Users/validate", loginRequest); // ← ajusta la ruta

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (result?.IsValid == true)
                {
                    _authData = result;
                    Username = username; // Guardamos el nombre de usuario

                    return true;
                }
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    public void Logout()
    {
        _authData = null;
        Username = null;
    }

    public async Task<HttpResponseMessage> SendAuthorizedAsync(HttpRequestMessage request)
    {
        // Añadir token actual
        if (IsAuthenticated && !string.IsNullOrEmpty(AccessToken))
        {
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken);
        }

        var response = await _httpClient.SendAsync(request);

        return response;
    }

    public async Task<bool> RefreshAccessTokenAsync()
    {
        if (string.IsNullOrEmpty(RefreshToken))
            return false;

        try
        {
            var refreshRequest = new { refreshToken = RefreshToken };
            var response = await _httpClient.PostAsJsonAsync("/api/Users/refresh", refreshRequest);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (result?.IsValid == true)
                {
                    // Actualizar solo el AccessToken (mantener RefreshToken si no cambia)
                    _authData.IsValid = true;
                    _authData.AccessToken = result.AccessToken;
                    _authData.RefreshToken = result.RefreshToken ?? RefreshToken;

                    return true;
                }
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    public int? GetUserId()
    {
        if (string.IsNullOrEmpty(AccessToken))
            return null;

        // Primero intenta con "user_id" (tu claim personalizada)
        var userId = _jwtParser.GetClaimAsInt(AccessToken, "user_id");
        if (userId.HasValue)
            return userId;

        // Luego intenta con "sub" (estándar)
        return _jwtParser.GetClaimAsInt(AccessToken, "sub");
    }
}