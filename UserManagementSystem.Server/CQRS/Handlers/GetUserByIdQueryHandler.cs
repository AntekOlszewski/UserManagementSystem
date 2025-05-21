using MediatR;
using UserManagementSystem.Server.CQRS.Queries;
using UserManagementSystem.Server.DTOs;
using UserManagementSystem.Server.Exceptions;
using UserManagementSystem.Server.Mappers;
using UserManagementSystem.Server.Repositiories;

namespace UserManagementSystem.Server.CQRS.Handlers;

public class GetUserByIdQueryHandler(IUserRepository repository) : IRequestHandler<GetUserByIdQuery, UserResponseDTO>
{
    private readonly IUserRepository _repository = repository;

    public async Task<UserResponseDTO> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(request.Id) ?? throw new UserNotFoundException(request.Id);
        var dto = UserMapper.ToDTO(user);
        return dto;
    }
}