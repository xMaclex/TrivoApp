using Application.Auth.Commands.Login;
using Application.Auth.Commands.Register;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var result = await _mediator.Send(
            new RegisterCommand(dto.Name, dto.Email, dto.Password)
        );
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await _mediator.Send(
            new LoginCommand(dto.Email, dto.Password)
        );
        return Ok(result);
    }
}