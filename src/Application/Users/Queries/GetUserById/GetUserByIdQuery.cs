using Application.DTOs;
using MediatR;

namespace Application.Users.Queries.GetUserByid;

public record GetUserByIdQuery(int Id) : IRequest<UserResponseDto?>;