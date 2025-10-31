using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using BlazorApp.Models;

namespace BlazorApp.Services;

public class AuthenticationService
{
    private readonly HttpClient _httpClient;
    private LoginResponse? _authData;

    public bool IsAuthenticated => !string.IsNullOrEmpty(_authData?.AccessToken) && _authData.IsValid;
    public string? AccessToken => _authData?.AccessToken;
    public string? RefreshToken => _authData?.RefreshToken;
    public string? Role => _authData?.Role;
    public string? Username { get; private set; } // Opcional: si tu API no devuelve username, podrías guardarlo tú


    // ✅ Inyectamos UserProfileService
    public AuthenticationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
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
        if (IsAuthenticated && !string.IsNullOrEmpty(AccessToken))
        {
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken);
        }
        return await _httpClient.SendAsync(request);
    }
}