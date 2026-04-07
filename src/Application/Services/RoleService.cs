using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class RoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<List<RoleResponseDto>> GetAllAsync()
    {
        var roles = await _roleRepository.GetAllAsync();
        return roles.Select(r => new RoleResponseDto
        {
            Id = r.Id,
            Name = r.Name
        }).ToList();
    }

    public async Task<RoleResponseDto> CreateAsync(CreateRoleDto dto)
    {
        var role = new Role { Name = dto.Name };
        await _roleRepository.AddAsync(role);
        await _roleRepository.SaveChangesAsync();

        return new RoleResponseDto
        {
            Id = role.Id,
            Name = role.Name
        };
    }
}