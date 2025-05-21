using MediatR;
using UserManagementSystem.Server.DTOs;

namespace UserManagementSystem.Server.CQRS.Commands;

public record UpdateUserCommand(Guid Id, UpdateUserDTO UpdateUserDTO) : IRequest<UserResponseDTO>;