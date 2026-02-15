using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class QuoteOrder : EntityBase
{
    public DateOnly PresentationDate { get; private set; }
    private readonly List<Document> _documents = new();
    public IReadOnlyCollection<Document> Documents => _documents;
    public Customer Customer { get; private set; }
    public Guid CustomerId { get; private set; }
    public Employee Employee { get; private set; }
    public Guid EmployeeId { get; private set; }
    public QuoteState QuoteState { get; private set; }
    public Quote Quote { get; private set; }

    public QuoteOrder(string path, Customer customer, Employee employee, QuoteState quoteState)
    {
        PresentationDate = new DateOnly();
        Customer = customer;
        CustomerId = customer.Id;
        Employee = employee;
        EmployeeId = employee.Id;
        QuoteState = quoteState;
    }

    public QuoteOrder()
    {
    }

    public void AddQuote(Quote quote) => Quote = quote;
    public void AddDocument(Document document) => _documents.Add(document);
}