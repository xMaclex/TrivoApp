using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;


namespace Application.Auth.Commands.Register;

public class RegisterHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IActivityLogRepository _activityLogRepository;
    private readonly ITokenService _tokenService;

    public RegisterHandler(IUserRepository userRepository, 
    IActivityLogRepository activityLogRepository, 
    ITokenService tokenService)
    {
        _userRepository = userRepository;
        _activityLogRepository = activityLogRepository;
        _tokenService = tokenService;
    }
    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var Existing = await _userRepository.GetByEmailAsync(request.Email!);
        if(Existing != null)
        {       
            throw new InvalidOperationException("El correo ya está registrado");
        }
            // Hashear el password con BCrypt
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = passwordHash,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        var token = _tokenService.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            Name = user.Name,
            Email = user.Email,
            Roles = new List<string>(),
            ExpiresAt = DateTime.UtcNow.AddHours(8)
        };
    }
}   