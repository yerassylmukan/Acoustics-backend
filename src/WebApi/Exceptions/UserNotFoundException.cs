namespace WebApi.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string userName) : base($"User not found with: {userName}")
    {
    }

    public UserNotFoundException() : base("User not found")
    {
    }
}