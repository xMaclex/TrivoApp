using Domain.Entities;

namespace Domain.Interfaces;

public interface IRoleRepository
{
    Task<List<Role>> GetAllAsync();
    Task<Role?> GetByIdAsync(int id);
    Task AddAsync(Role role);
    Task SaveChangesAsync();
}