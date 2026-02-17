namespace MyProject.Domain.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException() : base("User does not have permission to access this resource.")
    {
    }

    public ForbiddenException(string message) : base(message)
    {
    }
}
