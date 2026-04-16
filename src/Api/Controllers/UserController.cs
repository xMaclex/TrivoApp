using Application.DTOs;
using Application.Users.Commands.AssignRole;
using Application.Users.Commands.CreateUser;
using Application.Users.Commands.DeleteUser;
using Application.Users.Commands.UpdateUser;
using Application.Users.Queries.GetAllUsers;
using Application.Users.Queries.GetUserByid;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // todos los endpoints requieren token
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var result = await _mediator.Send(new GetAllUsersQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var result = await _mediator.Send(new GetUserByIdQuery(id));
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")] // solo admins pueden crear usuarios
    public async Task<IActionResult> CreateUser(CreateUserDto dto)
    {
        var result = await _mediator.Send(new CreateUserCommand(dto.Name, dto.Email));
        return Created($"/api/user/{result.Id}", result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUser(int id, CreateUserDto dto)
    {
        var result = await _mediator.Send(new UpdateUserCommand(id, dto.Name, dto.Email));
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var deleted = await _mediator.Send(new DeleteUserCommand(id));
        if (!deleted) return NotFound();
        return NoContent();
    }

    [HttpPost("{id}/roles")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignRole(int id, AssignRoleDto dto)
    {
        var assigned = await _mediator.Send(new AssignRoleCommand(id, dto.RoleId));
        if (!assigned)
            return BadRequest(new { message = "No se pudo asignar el rol." });

        return Ok(new { message = "Rol asignado correctamente" });
    }
}