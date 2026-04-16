using Application.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.Auth.Commands.Login;

public class LoginHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IActivityLogRepository _activityLogRepository;
    private readonly ITokenService _tokenService;

    public LoginHandler(IUserRepository userRepository,
    IActivityLogRepository activityLogRepository, 
    ITokenService tokenService)
    {
        _userRepository = userRepository;
        _activityLogRepository = activityLogRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthResponseDto> Handle(LoginCommand request, 
    CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email!);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Credenciales incorrectas");
        }
        
        if(!user.IsActive)
        {
            throw new UnauthorizedAccessException("Usuario Inactivo");
        }

        bool passwordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if(!passwordValid)
        {
            throw new UnauthorizedAccessException("Credenciales incorrectas");
        }
        
        var log = new Domain.Entities.ActivityLog
        {
            Action = "Usuario Logueado",
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow
        };
        await _activityLogRepository.AddAsync(log);
        await _activityLogRepository.SaveChangesAsync();

        var token = _tokenService.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            Name = user.Name,
            Email = user.Email,
            Roles = user.UserRoles
                .Select(ur => ur.Role?.Name ?? "")
                .ToList(),
            ExpiresAt = DateTime.UtcNow.AddHours(8)
        };
    }
}