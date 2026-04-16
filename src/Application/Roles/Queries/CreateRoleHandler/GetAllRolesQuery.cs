using Application.DTOs;
using MediatR;

namespace Application.Roles.Queries.GetAllRoles;
public record GetAllRolesQuery : IRequest<List<RoleResponseDto>>;