using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using Application.Shared;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;

namespace Application.Services;

public class QuoteService : IQuoteService
{
    private readonly IRepository _repository;
    private readonly IFileStorage _fileStorage;
    public QuoteService(IRepository repository, IFileStorage fileStorage)
    {
        _repository = repository;
        _fileStorage = fileStorage;
    }
    public async Task<QuoteOrderDto.Response> Register(QuoteOrderDto.RequestStream quoteDto, string idUser)
    {
        var idUserAuthenticated = idUser.ValidateId();
        var userAuthenticated = await _repository.GetForId<User>(idUserAuthenticated, nameof(Employee));
        if (userAuthenticated == null) throw new EntityNotFoundException($"User autenticathed with id {idUserAuthenticated} is not found");
        var idCustomer = quoteDto.idCustomer.ValidateId();
        var customer = await _repository.GetForId<Customer>(idCustomer);
        if (customer == null) throw new EntityNotFoundException($"Customer with id {idCustomer} is not found");
        var newQuote = new QuoteOrder(customer, userAuthenticated.Employee, QuoteState.EARRING);
        var namePathFile = await _fileStorage.SaveFile(quoteDto.file, quoteDto.name, quoteDto.contentType);
        var newDocument = new Document(namePathFile, newQuote);
        newQuote.AddDocument(newDocument);
        await _repository.Add(newQuote);
        var customerDto = new CustomerDto.ResponseWithQuote(customer.Id.ToString(), customer.FirstName, customer.LastName,
             customer.Dni.Value, customer.PhoneNumber);
        var employeeDto = new EmployeeDto.ReponseWithQuote(userAuthenticated.Employee.Id.ToString(), userAuthenticated.Employee.FirstName
            , userAuthenticated.Employee.LastName);
        return new QuoteOrderDto.Response(customerDto, employeeDto, newQuote.PresentationDate);
    }

    public async Task<QuoteOrderDto.Response> GetById(string id)
    {
        var idQuote = id.ValidateId();
        var quoteFound = await _repository.GetForId<QuoteOrder>(idQuote, nameof(Document), nameof(Customer), nameof(Employee));
        if (quoteFound == null) throw new EntityNotFoundException($"The quote order with id {idQuote} is not found");
        var customerDto = new CustomerDto.ResponseWithQuote(quoteFound.Customer.Id.ToString(), quoteFound.Customer.FirstName, quoteFound.Customer.LastName,
             quoteFound.Customer.Dni.Value, quoteFound.Customer.PhoneNumber);
        var employeeDto = new EmployeeDto.ReponseWithQuote(quoteFound.Employee.Id.ToString(), quoteFound.Employee.FirstName, quoteFound.Employee.LastName);
        var quoteDto = new QuoteOrderDto.Response(customerDto, employeeDto, quoteFound.PresentationDate);
        if (!quoteFound.Documents.Any()) return quoteDto;
        return quoteDto;
    }

    public Task<QuoteOrderDto.Response> Update(QuoteOrderDto.RequestUpdate quoteDto)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(string id)
    {
        var idQuote = id.ValidateId();
        var quoteFound = await _repository.GetForId<QuoteOrder>(idQuote);
        if (quoteFound == null) throw new EntityNotFoundException($"The quote order with id {idQuote} is not found");
        await _repository.Delete(quoteFound);
    }

    public async Task<List<QuoteOrderDto.Response>> GetAll()
    {
        var quoteOrders = await _repository.ListAll<QuoteOrder>(nameof(Customer), nameof(Employee));
        if (!quoteOrders.Any()) return [];
        return quoteOrders.Select(qo => new QuoteOrderDto.Response(new CustomerDto.ResponseWithQuote(qo.Customer.Id.ToString(),
            qo.Customer.FirstName, qo.Customer.LastName, qo.Customer.Dni.Value, qo.Customer.PhoneNumber),
            new EmployeeDto.ReponseWithQuote(qo.Employee.Id.ToString(), qo.Employee.FirstName, qo.Employee.LastName),
            qo.PresentationDate)).ToList();
    }
}