using Domain.Common;

namespace Domain.Entities;

public class Document : EntityBase
{
    public string Name { get; private set; }
    public string Path { get; private set; }
    public long Size { get; private set; }
    public string Type { get; private set; }
    public QuoteOrder QuoteOrder  { get; private set; }
    public Guid  QuoteOrderId { get; private set; } 

    public Document()
    {
    }

    public Document(string name, string path, long size, string type, QuoteOrder  quoteOrder)
    {
        Name = name;
        Path = path;
        Size = size;
        Type = type;
        QuoteOrder = quoteOrder;
        QuoteOrderId = quoteOrder.Id;
    }
}