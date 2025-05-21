using MediatR;

namespace UserManagementSystem.Server.CQRS.Commands;

public record DeleteUserByIdCommand(Guid Id) : IRequest;
