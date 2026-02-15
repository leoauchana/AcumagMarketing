using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities;

public abstract class Person : EntityBase
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Email Email { get; private set; }
    public Dni Dni { get; private set; }
    public Domicilie Domicilie { get; private set; }

    protected Person()
    {
    }

    protected Person(string firstName, string lastName, Email email, Dni dni, Domicilie domicilie)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Dni = dni;
        Domicilie = domicilie;
    }
}