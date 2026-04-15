using MediatR;

namespace Application.Users.Commands.AssignRole;

public record AssignRoleCommand(int UserId, int RoleId) : IRequest<bool>;

