using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class QuoteOrder : EntityBase
{
    public DateOnly Date { get; private set; }
    public string Path { get; private set; }
    public Customer Customer { get; private set; }
    public Guid CustomerId { get; private set; }
    public Employee Employee { get; private set; }
    public Guid EmployeeId { get; private set; }
    public QuoteState QuoteState { get; private set; }

    public QuoteOrder(string path, Customer customer, Employee employee, QuoteState quoteState)
    {
        Date = new DateOnly();
        Path = path;
        Customer = customer;
        CustomerId = customer.Id;
        Employee = employee;
        EmployeeId = employee.Id;
        QuoteState = quoteState;
    }
}