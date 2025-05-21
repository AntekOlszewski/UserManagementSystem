using UserManagementSystem.Server.Models;

namespace UserManagementSystem.Server.Repositiories;

public class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> _users =
    [
        new User
        {
            Id = Guid.Parse("c8adf962-89ed-454b-a0ad-1c5ec3c7486c"),
            Name = "Antek",
            Email = "antek@email.com",
            Avatar = "https://img.freepik.com/premium-vector/character-avatar-isolated_729149-194801.jpg?semt=ais_hybrid&w=740"
        },
        new User{
            Id = Guid.Parse("646ffde4-1a00-4158-917b-cf8d0283dc19"),
            Name = "User",
            Email = "user@email.com",
            Avatar = "https://www.svgrepo.com/show/384670/account-avatar-profile-user.svg"
        },
        new User{
            Id = Guid.Parse("29729133-0890-480f-bf16-613ef79a53fc"),
            Name = "User2",
            Email = "user2@email.com",
        }
    ];

    public Task<IEnumerable<User>> GetAllAsync()
        => Task.FromResult(_users.AsEnumerable());

    public Task<User?> GetByIdAsync(Guid id)
        => Task.FromResult(_users.FirstOrDefault(u => u.Id == id));

    public Task<User?> GetByEmailAsync(string email)
        => Task.FromResult(_users.FirstOrDefault(u => u.Email == email));

    public Task AddAsync(User user)
    {
        _users.Add(user);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(User user)
    {
        var index = _users.FindIndex(u => u.Id == user.Id);
        if (index != -1)
        {
            _users[index] = user;
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user is not null)
        {
            _users.Remove(user);
        }
        return Task.CompletedTask;
    }
}
