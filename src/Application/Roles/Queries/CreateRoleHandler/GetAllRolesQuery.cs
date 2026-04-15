using Application.DTOs;
using MediatR;

public record GetAllRolesQuery : IRequest<List<RoleResponseDto>>;