using MediatR;
using UserManagementSystem.Server.DTOs;

namespace UserManagementSystem.Server.CQRS.Queries;

public record GetUserByIdQuery(Guid Id) : IRequest<UserResponseDTO>;