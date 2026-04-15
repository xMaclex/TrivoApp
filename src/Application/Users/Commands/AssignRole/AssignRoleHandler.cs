using Domain.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Users.Commands.AssignRole;


public class AssignRoleHandler : IRequestHandler<AssignRoleCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IActivityLogRepository _activityLogRepository;

    public AssignRoleHandler(IUserRepository userRepository, IRoleRepository roleRepository, IActivityLogRepository activityLogRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _activityLogRepository = activityLogRepository;
    }
    public async Task<bool> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null) return false;

        var role = await _roleRepository.GetByIdAsync(request.RoleId);
        if (role == null) return false;

        bool alreadyHasRole = user.UserRoles.Any(ur => ur.RoleId == role.Id);
        if (alreadyHasRole) return false;

        user.UserRoles.Add(new UserRole 
        {
             UserId = user.Id, 
             RoleId = role.Id    
        });

        var log = new ActivityLog
        {
            Action = $"Rol '{role.Name}' asignado al usuario '{user.Name}'",
            UserId = request.UserId,
            CreatedAt = DateTime.UtcNow
        };
        await _activityLogRepository.AddAsync(log);
        await _activityLogRepository.SaveChangesAsync();
        return true;
    }
}
