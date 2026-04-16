using Application.DTOs;
using MediatR;

namespace Application.Auth.Commands.Register;

public record RegisterCommand(string? Name, string? Email, string? Password) : IRequest<AuthResponseDto>;