using Application.DTOs;
using Domain.Interfaces;
using MediatR;


namespace Application.Users.Queries.GetAllUsers;

public class GetAllUserHandler : IRequestHandler<GetAllUsersQuery, List<UserResponseDto>>
{
    private readonly IUserRepository _userRepository;

    public GetAllUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;   
    }

    public async Task<List<UserResponseDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAsync();

        return users.Select(u => new UserResponseDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            CreatedAt = u.CreatedAt,
            Roles = u.UserRoles
                .Select(ur => ur.Role?.Name?? "")
                .ToList(),
            RecentActivity = u.ActivityLogs
                .OrderByDescending(a => a.CreatedAt)
                .Select(a => new ActivityLogDto
                {
                    Action = a.Action,
                    CreatedAt = a.CreatedAt
                })
                .ToList()      
        }) .ToList();
    }
}