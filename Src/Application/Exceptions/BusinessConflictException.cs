namespace Application.Exceptions;

public class BusinessConflictException : ApplicationException
{
    public BusinessConflictException(string msg) : base(msg)
    {
    }
}