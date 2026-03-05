using Application.DTOs;

namespace Application.Interfaces;

public interface IQuoteService
{
    Task<QuoteOrderDto.Response> Register(QuoteOrderDto.RequestStream quoteDto, string idUser);
    Task<QuoteOrderDto.Response> GetById(string id);
    Task<QuoteOrderDto.Response> Update(QuoteOrderDto.RequestUpdate quoteDto);
    Task Delete(string id);
    Task<List<QuoteOrderDto.Response>> GetAll();
}