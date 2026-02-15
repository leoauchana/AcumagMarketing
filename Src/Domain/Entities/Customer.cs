using Domain.ValueObjects;

namespace Domain.Entities;

public class Customer : Person
{
    public string PhoneNumber { get; private set; }
    private readonly List<QuoteOrder> _orders = new();
    public IReadOnlyCollection<QuoteOrder> QuoteOrders => _orders;
    public Customer()
    {
    }
    public Customer(string firstName, string lastName, Email email, Dni dni, Domicilie domicilie,
        string phoneNumber) : base(firstName,
        lastName, email, dni, domicilie)
    {
        PhoneNumber = phoneNumber;
    }
}