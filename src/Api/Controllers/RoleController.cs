using Application.DTOs;
using Application.Roles.Commands.CreateRole;
using Application.Roles.Queries.GetAllRoles;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // todos los endpoints requieren token
public class RoleController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        var result = await _mediator.Send(new GetAllRolesQuery());
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateRole(CreateRoleDto dto)
    {
        var result = await _mediator.Send(new CreateRoleCommand(dto.Name));
        return Created($"/api/role/{result.Id}", result);
    }
}
