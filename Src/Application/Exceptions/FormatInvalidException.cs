namespace Application.Exceptions;

public class FormatInvalidException : ApplicationException
{
    public FormatInvalidException(string msg) : base(msg)
    {
    }
}