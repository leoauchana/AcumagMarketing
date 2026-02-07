namespace Domain.ValueObjects;

public class Dni
{
    public string Value { get; private set; }

    private Dni(string value)
    {
        Value = value;
    }
    
    public Dni Create(string value)
    {
        return new Dni(value);
    }
}