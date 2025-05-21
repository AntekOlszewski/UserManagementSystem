using Moq;
using UserManagementSystem.Server.CQRS.Handlers;
using UserManagementSystem.Server.CQRS.Queries;
using UserManagementSystem.Server.Exceptions;
using UserManagementSystem.Server.Models;
using UserManagementSystem.Server.Repositiories;

namespace UserManagementSystemTests;

public class GetUserByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsUserDTO_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, Name = "Test", Email = "test@example.com" };

        var repositoryMock = new Mock<IUserRepository>();
        repositoryMock.Setup(r => r.GetByIdAsync(userId))
                      .ReturnsAsync(user);

        var handler = new GetUserByIdQueryHandler(repositoryMock.Object);

        // Act
        var result = await handler.Handle(new GetUserByIdQuery(userId), CancellationToken.None);

        // Assert
        Assert.Equal(userId, result.Id);
        Assert.Equal(user.Name, result.Name);
    }

    [Fact]
    public async Task Handle_ThrowsUserNotFoundException_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var repositoryMock = new Mock<IUserRepository>();
        repositoryMock.Setup(r => r.GetByIdAsync(userId))
                      .ReturnsAsync((User?)null);

        var handler = new GetUserByIdQueryHandler(repositoryMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(() =>
            handler.Handle(new GetUserByIdQuery(userId), CancellationToken.None));
    }
}
