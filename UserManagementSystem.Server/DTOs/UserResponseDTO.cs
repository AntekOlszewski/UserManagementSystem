namespace UserManagementSystem.Server.DTOs;

public class UserResponseDTO
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Avatar { get; set; }
}
