namespace Application.Exceptions;

public class ApplicationException : Exception
{
    public ApplicationException(string msg) : base(msg)
    {
    }
}