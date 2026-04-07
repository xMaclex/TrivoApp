using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class UserServices
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IActivityLogRepository _activityLogRepository;

    public UserServices(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IActivityLogRepository activityLogRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _activityLogRepository = activityLogRepository;
    }

    public async Task<List<UserResponseDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return users.Select(u => MapToDto(u)).ToList();
    }

    public async Task<UserResponseDto?> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return null;

        return MapToDto(user);
    }

    public async Task<UserResponseDto> AddAsync(CreateUserDto dto)
    {
        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        // El log se registra después de SaveChanges porque necesitamos el Id del user
        var log = new ActivityLog
        {
            Action = $"Usuario '{dto.Name}' creado",
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow
        };
        await _activityLogRepository.AddAsync(log);
        await _activityLogRepository.SaveChangesAsync();

        return MapToDto(user);
    }

    public async Task<UserResponseDto?> UpdateAsync(int id, CreateUserDto dto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return null;

        user.Name = dto.Name;
        user.Email = dto.Email;

        await _userRepository.UpdateAsync(user);

        var log = new ActivityLog
        {
            Action = $"Usuario actualizado: nombre='{dto.Name}', email='{dto.Email}'",
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow
        };
        await _activityLogRepository.AddAsync(log);

        await _userRepository.SaveChangesAsync();

        return MapToDto(user);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return false;

        await _userRepository.DeleteAsync(user);
        await _userRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AssignRoleAsync(int userId, AssignRoleDto dto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return false;

        var role = await _roleRepository.GetByIdAsync(dto.RoleId);
        if (role == null) return false;

        // Verificar con LINQ si ya tiene ese rol
        bool alreadyHasRole = user.UserRoles.Any(ur => ur.RoleId == dto.RoleId);
        if (alreadyHasRole) return false;

        user.UserRoles.Add(new UserRole
        {
            UserId = userId,
            RoleId = dto.RoleId,
            AssignedAt = DateTime.UtcNow
        });

        var log = new ActivityLog
        {
            Action = $"Rol '{role.Name}' asignado al usuario",
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };
        await _activityLogRepository.AddAsync(log);

        await _userRepository.SaveChangesAsync();
        return true;
    }

    // Método privado para mapear entidad a DTO (evita repetir código)
    private static UserResponseDto MapToDto(User user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Roles = user.UserRoles
                .Select(ur => ur.Role?.Name ?? "")
                .ToList(),
            RecentActivity = user.ActivityLogs
                .OrderByDescending(a => a.CreatedAt)
                .Select(a => new ActivityLogDto
                {
                    Action = a.Action,
                    CreatedAt = a.CreatedAt
                })
                .ToList()
        };
    }
}