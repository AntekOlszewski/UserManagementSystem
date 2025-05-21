namespace UserManagementSystem.Server.Exceptions;

public class EmailAlreadyExistsException(string email) : Exception($"User with email '{email}' already exists.")
{
}
