using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly IRoleRepository _roleRepository;
    public RoleController(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await _roleRepository.GetAllAsync();
        return Ok(roles);
    }
    [HttpPost]
    public async Task<IActionResult> CreateRoles(CreateRoleDto dto)
    {
        var role = new Role { Name = dto.Name };
        await _roleRepository.AddAsync(role);
        await _roleRepository.SaveChangesAsync();
        return Created($"/api/role/{role.Id}", role);
    }

}