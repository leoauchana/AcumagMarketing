using Application.DTOs;
using Application.Interfaces;
using Domain.Interfaces;

namespace Application.Services;

public class QuoteService : IQuoteService
{
    private readonly IRepository _repository;
    public QuoteService(IRepository repository)
    {
        _repository  = repository;
    }
    public Task Register(QuoteDto.Request quoteDto)
    {
        throw new NotImplementedException();
    }

    public Task<QuoteDto.Response> GetById(string id)
    {
        throw new NotImplementedException();
    }

    public Task<QuoteDto.Response> Update(QuoteDto.RequestUpdate quoteDto)
    {
        throw new NotImplementedException();
    }

    public Task<QuoteDto.Response> Delete(string id)
    {
        throw new NotImplementedException();
    }

    public Task<QuoteDto.Response> GetAll()
    {
        throw new NotImplementedException();
    }
}