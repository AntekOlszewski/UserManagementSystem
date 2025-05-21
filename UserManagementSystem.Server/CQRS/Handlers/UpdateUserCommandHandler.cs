using MediatR;
using UserManagementSystem.Server.CQRS.Commands;
using UserManagementSystem.Server.DTOs;
using UserManagementSystem.Server.Exceptions;
using UserManagementSystem.Server.Mappers;
using UserManagementSystem.Server.Repositiories;

namespace UserManagementSystem.Server.CQRS.Handlers;

public class UpdateUserCommandHandler(IUserRepository repository) : IRequestHandler<UpdateUserCommand, UserResponseDTO>
{
    private readonly IUserRepository _repository = repository;
    public async Task<UserResponseDTO> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(request.Id) ?? throw new UserNotFoundException(request.Id);

        var existingUser = await _repository.GetByEmailAsync(request.UpdateUserDTO.Email);
        if (existingUser != null && existingUser.Id != request.Id)
        {
            throw new EmailAlreadyExistsException(request.UpdateUserDTO.Email);
        }

        user.Name = request.UpdateUserDTO.Name;
        user.Email = request.UpdateUserDTO.Email;
        user.Avatar = request.UpdateUserDTO.Avatar;

        await _repository.UpdateAsync(user);
        return UserMapper.ToDTO(user);
    }
}
