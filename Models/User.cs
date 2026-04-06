namespace UserApi.Models;
public class User
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }

    public ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
