namespace Domain.Entities;
public class User
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }

    // One-to-Many: un usuario tiene muchos logs de actividad
    public ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
    //Many-to-Many con Role a través de UserRole
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
