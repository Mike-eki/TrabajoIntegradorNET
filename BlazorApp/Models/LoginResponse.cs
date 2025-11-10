namespace BlazorApp.Models;

public class LoginResponse
{
    public bool IsValid { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public string? Role { get; set; }
}
