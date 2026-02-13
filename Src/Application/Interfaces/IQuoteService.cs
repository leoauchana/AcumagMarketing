using Application.DTOs;

namespace Application.Interfaces;

public interface IQuoteService
{
    Task Register(QuoteDto.Request quoteDto);
    Task<QuoteDto.Response> GetById(string id);
    Task<QuoteDto.Response> Update(QuoteDto.RequestUpdate quoteDto);
    Task<QuoteDto.Response> Delete(string id);
    Task<QuoteDto.Response> GetAll();
}