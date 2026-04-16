namespace Application.DTOs;

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string? Email { get; set; }
    public List<string> Roles { get; set; } = new();
    public DateTime ExpiresAt { get; set; }
}