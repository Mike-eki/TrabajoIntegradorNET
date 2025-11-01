using Models.DTOs;

namespace BlazorApp.Models;

public class CareerResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<SubjectSimpleDto> Subjects { get; set; } = new();
}
