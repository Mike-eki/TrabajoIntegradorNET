using BlazorApp.Models;
using System.Net.Http.Json;

namespace BlazorApp.Services;

public class CareerService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationService _authService;

    public CareerService(HttpClient httpClient, AuthenticationService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    public async Task<List<CareerResponseDto>?> GetCareersAsync()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/Careers");
            var response = await _authService.SendAuthorizedAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<CareerResponseDto>>();
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> EnrollInCareerAsync(int userId, int careerId)
    {
        try
        {
            var careerIds = new List<int> { careerId };
            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/Careers/{userId}/careers");
            request.Content = JsonContent.Create(careerIds); // Serializa como JSON

            var response = await _authService.SendAuthorizedAsync(request);

            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

}