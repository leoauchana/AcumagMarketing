using Domain.ValueObjects;

namespace Domain.Entities;

public class Employee : Person
{
    public Role Role { get; private set; }
    public Guid RoleId { get; private set; }
    public User User { get; private set; }
    public Guid UserId { get; private set; }
    private readonly List<QuoteOrder> _orders = new();
    public IReadOnlyCollection<QuoteOrder> QuoteOrders => _orders;

    public Employee()
    {
    }

    public Employee(string firstName, string lastName, Email email, Dni dni, Domicilie domicilie, Role role, User user)
        : base(firstName, lastName,
            email, dni, domicilie)
    {
        Role = role;
        RoleId = role.Id;
        User = user;
        UserId = user.Id;
    }
    public void UpdateRole(Role role) => Role = role;
}