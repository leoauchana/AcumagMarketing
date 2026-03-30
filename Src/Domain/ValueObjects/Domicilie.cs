namespace Domain.ValueObjects;

public class Domicilie
{
    public string Street { get; private set; }
    public string City { get; private set; }
    public int Number { get; private set; }
    public string ZipCode { get; private set; }

    public Domicilie(string street, string city, int number, string zipCode)
    {
        Street = street;
        City = city;
        Number = number;
        ZipCode = zipCode;
    }
}