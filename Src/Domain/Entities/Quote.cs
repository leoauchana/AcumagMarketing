using Domain.Common;

namespace Domain.Entities;

public class Quote : EntityBase
{
    public DateOnly QuoteDate { get; private set; }
    public DateOnly ExpirationDate { get; private set; }
    public string Observations { get; private set; }
    public QuoteOrder QuoteOrder { get; private set; }
    public Guid QuoteOrderId { get; private set; }
    public Employee Employee { get; private set; }
    public Guid EmployeeId { get; private set; }

    public Quote()
    {
    }

    public Quote(QuoteOrder quoteOrder, Employee employee, DateOnly expirationDate, string observations)
    {
        QuoteDate = new DateOnly();
        ExpirationDate = expirationDate;
        Observations = observations;
        QuoteOrder = quoteOrder;
        QuoteOrderId = quoteOrder.Id;
        Employee = employee;
        EmployeeId = employee.Id;
    }
}