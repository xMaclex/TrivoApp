using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Roles.Commands.CreateRole;

public class CreateRoleHandler : IRequestHandler<CreateRoleCommand, RoleResponseDto>
{
    private readonly IRoleRepository _roleRepository;

    public CreateRoleHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<RoleResponseDto> Handle(
        CreateRoleCommand request,
        CancellationToken cancellationToken)
    {
        var role = new Role { Name = request.Name };
        await _roleRepository.AddAsync(role);
        await _roleRepository.SaveChangesAsync();

        return new RoleResponseDto
        {
            Id = role.Id,
            Name = role.Name
        };
    }
}