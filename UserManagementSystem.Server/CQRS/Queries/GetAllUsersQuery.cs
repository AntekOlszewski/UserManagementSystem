using MediatR;
using UserManagementSystem.Server.DTOs;

namespace UserManagementSystem.Server.CQRS.Queries;

public record GetAllUsersQuery() : IRequest<IEnumerable<UserResponseDTO>>;