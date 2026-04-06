using Microsoft.EntityFrameworkCore;
using UserApi.Data;
using UserApi.Models;
using UserApi.Repositories.Interfaces;

namespace UserApi.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _context;

    public RoleRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Role>> GetAllAsync()
    {
        return await _context.Roles.ToListAsync();
    }
    public async Task<Role?> GetByIdAsync(int id)
    {
        return await _context.Roles.FindAsync(id);
    }
    public async Task AddAsync(Role role)
    {
        await _context.Roles.AddAsync(role);
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

}
