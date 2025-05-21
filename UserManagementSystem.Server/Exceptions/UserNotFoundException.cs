namespace UserManagementSystem.Server.Exceptions;

public class UserNotFoundException(Guid id) : Exception($"User with id '{id}' does not exists.")
{
}
