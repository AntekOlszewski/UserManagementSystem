using System.ComponentModel.DataAnnotations;

namespace UserManagementSystem.Server.DTOs;

public class UpdateUserDTO
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Url]
    public string? Avatar { get; set; }
}
