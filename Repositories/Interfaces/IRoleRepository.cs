using UserApi.Models;

namespace UserApi.Repositories.Interfaces;

public interface IRoleRepository
{
    Task<List<Role>> GetAllAsync();
    Task<Role?> GetByIdAsync(int id);
    Task AddAsync(Role role);
    Task SaveChangesAsync();
}