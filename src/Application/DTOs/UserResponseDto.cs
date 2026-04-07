using Domain.Entities;

namespace Application.DTOs;

public class UserResponseDto
{
    public int Id { get; set; }
    public string? Name { get; set;}
    public string? Email { get; set;}
    public List<string> Roles { get; set; } = new();
    public List<ActivityLogDto> RecentActivity { get; set; } = new();
}

public class ActivityLogDto
{
    public string? Action { get; set; }
    public DateTime CreatedAt { get; set; }
}