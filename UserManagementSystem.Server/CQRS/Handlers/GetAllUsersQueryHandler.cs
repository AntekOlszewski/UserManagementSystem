using MediatR;
using UserManagementSystem.Server.CQRS.Queries;
using UserManagementSystem.Server.DTOs;
using UserManagementSystem.Server.Mappers;
using UserManagementSystem.Server.Repositiories;

namespace UserManagementSystem.Server.CQRS.Handlers;

public class GetAllUsersQueryHandler(IUserRepository repository) : IRequestHandler<GetAllUsersQuery, IEnumerable<UserResponseDTO>>
{
    private readonly IUserRepository _repository = repository;

    public async Task<IEnumerable<UserResponseDTO>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _repository.GetAllAsync();
        var dtos = users.Select(u => UserMapper.ToDTO(u));
        return dtos;
    }
}