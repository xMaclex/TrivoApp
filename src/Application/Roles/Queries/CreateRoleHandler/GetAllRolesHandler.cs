using Application.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.Roles.Queries.GetAllRoles;


public class GetAllRolesHandler : IRequestHandler<GetAllRolesQuery, List<RoleResponseDto>>
{
    private readonly IRoleRepository _roleRepository;

    public GetAllRolesHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<List<RoleResponseDto>> Handle(
        GetAllRolesQuery request, 
        CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetAllAsync();

        return role.Select(r => new RoleResponseDto
        {
            Id = r.Id,
            Name = r.Name,
        }).ToList();
    }
}