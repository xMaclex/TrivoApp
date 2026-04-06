using UserApi.Models;

namespace UserApi.Repositories.Interfaces;

public interface IActivityLogRepository
{
    Task AddAsync(ActivityLog log);
    Task<List<ActivityLog>> GetByIdAsync(int id);
    Task SaveChangesAsync();
}   