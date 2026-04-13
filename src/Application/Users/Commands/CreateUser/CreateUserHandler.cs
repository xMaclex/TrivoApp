using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Users.Commands.CreateUser;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IActivityLogRepository _activityLogRepository;

    public CreateUserHandler (IUserRepository userRepository, IActivityLogRepository activityLogRepository)
    {
        _userRepository = userRepository;
        _activityLogRepository = activityLogRepository;
    }

    public async Task<UserResponseDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        var log = new ActivityLog
        {
          Action = $"Usuario '{user.Name}' Creado",
          UserId = user.Id,
          CreatedAt = DateTime.UtcNow  
        };
        await _activityLogRepository.AddAsync(log);
        await _activityLogRepository.SaveChangesAsync();

        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };
    }
}
