using Moq;
using UserManagementSystem.Server.CQRS.Commands;
using UserManagementSystem.Server.CQRS.Handlers;
using UserManagementSystem.Server.DTOs;
using UserManagementSystem.Server.Exceptions;
using UserManagementSystem.Server.Models;
using UserManagementSystem.Server.Repositiories;

namespace UserManagementSystemTests;

public class CreateUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _handler = new CreateUserCommandHandler(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_UserWithEmailExists_ThrowsEmailAlreadyExistsException()
    {
        // Arrange
        var existingUser = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            Name = "Existing User",
            Avatar = null
        };

        _userRepositoryMock
            .Setup(repo => repo.GetByEmailAsync(existingUser.Email))
            .ReturnsAsync(existingUser);

        var createUserDto = new CreateUserDTO
        {
            Email = existingUser.Email,
            Name = "New User",
            Avatar = null
        };

        var command = new CreateUserCommand(createUserDto);

        // Act & Assert
        await Assert.ThrowsAsync<EmailAlreadyExistsException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_UserDoesNotExist_AddsUserAndReturnsDto()
    {
        // Arrange
        var createUserDto = new CreateUserDTO
        {
            Email = "newuser@example.com",
            Name = "New User",
            Avatar = null
        };

        _userRepositoryMock
            .Setup(repo => repo.GetByEmailAsync(createUserDto.Email))
            .ReturnsAsync((User?)null);

        _userRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        var command = new CreateUserCommand(createUserDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _userRepositoryMock.Verify(repo => repo.AddAsync(It.Is<User>(u =>
            u.Email == createUserDto.Email &&
            u.Name == createUserDto.Name &&
            u.Avatar == createUserDto.Avatar)), Times.Once);

        Assert.Equal(createUserDto.Email, result.Email);
        Assert.Equal(createUserDto.Name, result.Name);
        Assert.Equal(createUserDto.Avatar, result.Avatar);
        Assert.NotEqual(Guid.Empty, result.Id);
    }
}
