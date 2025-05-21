using MediatR;
using UserManagementSystem.Server.CQRS.Commands;
using UserManagementSystem.Server.DTOs;
using UserManagementSystem.Server.Exceptions;
using UserManagementSystem.Server.Mappers;
using UserManagementSystem.Server.Models;
using UserManagementSystem.Server.Repositiories;

namespace UserManagementSystem.Server.CQRS.Handlers;

public class CreateUserCommandHandler(IUserRepository repository) : IRequestHandler<CreateUserCommand, UserResponseDTO>
{
    private readonly IUserRepository _repository = repository;

    public async Task<UserResponseDTO> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _repository.GetByEmailAsync(request.CreateUserDTO.Email);
        if (existingUser != null)
        {
            throw new EmailAlreadyExistsException(request.CreateUserDTO.Email);
        }
        var user = UserMapper.ToEntity(request.CreateUserDTO);

        await _repository.AddAsync(user);

        var dto = UserMapper.ToDTO(user);
        return dto;
    }
}