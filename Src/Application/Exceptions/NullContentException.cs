namespace Application.Exceptions;

public class NullContentException : ApplicationException
{
    public NullContentException(string msg) : base(msg)
    {
    }
}