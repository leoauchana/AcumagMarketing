namespace Domain.ValueObjects;

public class Email
{
    public string Value { get; private set; }

    private Email(string value)
    {
        Value = value;
    }

    public Email Create(string value)
    {
        return new Email(value);
    }
}