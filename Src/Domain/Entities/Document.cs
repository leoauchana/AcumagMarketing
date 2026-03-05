using Domain.Common;

namespace Domain.Entities;

public class Document : EntityBase
{
    public string Name { get; private set; }
    public QuoteOrder QuoteOrder  { get; private set; }
    public Guid  QuoteOrderId { get; private set; } 

    public Document()
    {
    }

    public Document(string name, QuoteOrder  quoteOrder)
    {
        Name = name;
        QuoteOrder = quoteOrder;
        QuoteOrderId = quoteOrder.Id;
    }
}