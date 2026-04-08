using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllAsync()
    {
        // Include carga las relaciones (LINQ con navegación)
        return await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Include(u => u.ActivityLogs.OrderByDescending(a => a.CreatedAt).Take(5))
            .ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Include(u => u.ActivityLogs.OrderByDescending(a => a.CreatedAt).Take(10))
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }
    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }
    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
    }

    public async Task DeleteAsync(User user)
    {
        _context.Users.Remove(user);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
