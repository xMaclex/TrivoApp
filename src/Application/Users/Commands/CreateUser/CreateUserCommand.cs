using Application.DTOs;
using MediatR;

namespace Application.Users.Commands.CreateUser;

public record CreateUserCommand(string? Name, string? Email) : IRequest<UserResponseDto>;