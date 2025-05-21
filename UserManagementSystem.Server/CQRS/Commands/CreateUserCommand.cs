using MediatR;
using System.ComponentModel.DataAnnotations;
using UserManagementSystem.Server.DTOs;

namespace UserManagementSystem.Server.CQRS.Commands;

public record CreateUserCommand(CreateUserDTO CreateUserDTO) : IRequest<UserResponseDTO>;