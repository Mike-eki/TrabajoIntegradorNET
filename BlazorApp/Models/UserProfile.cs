using Models.DTOs;

namespace BlazorApp.Models;

public class UserProfile
{
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }

    public List<CareerSimpleDto>? Careers { get; set; } = new List<CareerSimpleDto>();
    // Agrega otros campos según tu API
}
