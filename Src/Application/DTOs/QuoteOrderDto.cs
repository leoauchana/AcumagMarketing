using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.DTOs;

public class QuoteOrderDto
{
    public record RequestFormFile(IFormFile quoteFile, string idCustomer);
    public record RequestStream(Stream file, string name, string contentType, string idCustomer);
    public record Response(CustomerDto.ResponseWithQuote customer, EmployeeDto.ReponseWithQuote employee, DateOnly presentationDate);
    public record RequestUpdate();
}