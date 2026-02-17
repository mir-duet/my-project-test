namespace MyProject.Domain.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException() : base("User is not authenticated.")
    {
    }

    public UnauthorizedException(string message) : base(message)
    {
    }
}
