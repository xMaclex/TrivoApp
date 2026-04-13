using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;


namespace Application.Users.Commands.UpdateUser;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UserResponseDto?>
{
    private readonly IUserRepository _userRepository;
    private readonly IActivityLogRepository _activityLogRepository;

    public UpdateUserHandler (IUserRepository userRepository, IActivityLogRepository activityLogRepository)
    {
        _userRepository = userRepository;
        _activityLogRepository = activityLogRepository;
    }
    
    public async Task<UserResponseDto?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        if(user == null) return null;

        user.Name = request.Name;
        user.Email = request.Email;

        await _userRepository.UpdateAsync(user);


        var log = new ActivityLog
        {
          Action = $"Usuario Actualizado : Nombre = '{request.Name}'",
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
