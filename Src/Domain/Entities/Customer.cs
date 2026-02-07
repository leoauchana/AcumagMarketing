using Domain.ValueObjects;

namespace Domain.Entities;

public class Customer : Person
{
    public string PhoneNumber { get; private set; }

    public Customer(string firstName, string lastName, Email email, Dni dni, Domicilie domicilie, string phoneNumber) : base(firstName,
        lastName, email, dni, domicilie)
    {
        PhoneNumber = phoneNumber;
    }
}