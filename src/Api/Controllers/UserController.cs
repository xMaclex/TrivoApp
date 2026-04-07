using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Services;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserServices _userService;

    public UserController(UserServices userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserDto dto)
    {
        var user = await _userService.AddAsync(dto);
        return Created($"/api/user/{user.Id}", user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, CreateUserDto dto)
    {
        var user = await _userService.UpdateAsync(id, dto);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var deleted = await _userService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }

    // POST /api/user/1/roles
    [HttpPost("{id}/roles")]
    public async Task<IActionResult> AssignRole(int id, AssignRoleDto dto)
    {
        var assigned = await _userService.AssignRoleAsync(id, dto);
        if (!assigned)
            return BadRequest(new { message = "No se pudo asignar el rol. Verifica que el usuario y el rol existen, o que el usuario no tenga ya ese rol." });

        return Ok(new { message = "Rol asignado correctamente" });
    }
}