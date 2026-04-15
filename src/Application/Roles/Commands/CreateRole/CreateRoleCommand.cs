using Application.DTOs;
using MediatR;

namespace Application.Roles.Commands.CreateRole;

public record CreateRoleCommand(string? Name) : IRequest<RoleResponseDto>;