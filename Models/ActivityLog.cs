namespace UserApi.Models;

public class ActivityLog
{
    public int Id { get; set; }
    public string? Action { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Clave foránea hacia User
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}