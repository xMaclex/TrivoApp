using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories;

public class ActivityLogRepository: IActivityLogRepository
{
    private readonly AppDbContext _context;

    public ActivityLogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ActivityLog log)
    {
        await _context.activityLogs.AddAsync(log);
    } 

    public async Task<List<ActivityLog>> GetByIdAsync(int id)
    {
        return await _context.activityLogs
            .Where(a => a.UserId == id)
            .OrderByDescending(a => a.CreatedAt)
            .Take(10)
            .ToListAsync();
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}