using MediatR;
using UserManagementSystem.Server.CQRS.Commands;
using UserManagementSystem.Server.Repositiories;

namespace UserManagementSystem.Server.CQRS.Handlers;

public class DeleteUserByIdCommandHandler(IUserRepository repository) : IRequestHandler<DeleteUserByIdCommand>
{
    private readonly IUserRepository _repository = repository;

    public async Task Handle(DeleteUserByIdCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id);
    }
}
