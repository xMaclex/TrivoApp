using Application.DTOs;
using MediatR;

namespace Application.Users.Queries.GetAllUsers;

// IRequest<T> le dice a MediatR qué tipo va a devolver este Query
public record GetAllUsersQuery : IRequest<List<UserResponseDto>>;


