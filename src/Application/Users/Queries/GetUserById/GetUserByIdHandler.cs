using Application.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.Users.Queries.GetUserByid;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserResponseDto?>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponseDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        if (user == null) return null;

        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            //CreatedAt = user.CreatedAt,
            Roles = user.UserRoles
                .Select(ur => ur.Role?.Name ?? "")
                .ToList(),
            RecentActivity = user.ActivityLogs
                .OrderByDescending(a => a.CreatedAt)
                .Select(a => new ActivityLogDto
                {
                    Action = a.Action,
                    CreatedAt = a.CreatedAt
                })
                .ToList()
        };
    }
}