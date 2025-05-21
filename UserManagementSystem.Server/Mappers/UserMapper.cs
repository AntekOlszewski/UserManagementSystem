using UserManagementSystem.Server.DTOs;
using UserManagementSystem.Server.Models;
namespace UserManagementSystem.Server.Mappers;

public static class UserMapper
{
    public static User ToEntity(CreateUserDTO dto)
        => new User { Id = Guid.NewGuid(), Name = dto.Name, Email = dto.Email, Avatar = dto.Avatar };
    public static UserResponseDTO ToDTO(User user)
        => new UserResponseDTO { Id = user.Id, Name = user.Name, Email = user.Email, Avatar = user.Avatar };
}
