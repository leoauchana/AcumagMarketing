using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;

namespace Application.Services;

public class CustomersService : ICustomersService
{
    private readonly IRepository _repository;

    public CustomersService(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CustomerDto.Response>> GetAllCustomers()
    {
        var customers = await _repository.GetAll<Customer>();
        if (!(customers.Count > 0)) return [];
        return customers.Select(c => new CustomerDto.Response()).ToList();
    }

    public async Task<CustomerDto.Response?> GetCustomerById(string id)
    {
        if (string.IsNullOrEmpty(id) || !(Guid.TryParse(id, out Guid idCustomer)))
            throw new FormatInvalidException("The id is invalid");
        var customer = await _repository.GetForId<Customer>(idCustomer);
        if (customer == null) throw new EntityNotFoundException($"The customer with id {idCustomer} was not found");
        return new CustomerDto.Response();
    }

    public async Task<CustomerDto.Response?> Create(CustomerDto.Request customerDto)
    {
        var newCustomer = new Customer(customerDto.firstName, customerDto.lastName, Email.Create(customerDto.email),
            Dni.Create(customerDto.dni), new Domicilie(customerDto.street, customerDto.city,
                customerDto.number, customerDto.zipCode), customerDto.phoneNumber);
        await _repository.Add(newCustomer);
        return new CustomerDto.Response();
    }

    public async Task<CustomerDto.Response?> Update(CustomerDto.RequestUpdate customerDto)
    {
        if (string.IsNullOrEmpty(customerDto.id) || !(Guid.TryParse(customerDto.id, out Guid idCustomer)))
            throw new FormatInvalidException("The id is invalid");
        var customerToUpdate = await _repository.GetForId<Customer>(idCustomer);
        if (customerToUpdate == null) throw new EntityNotFoundException($"The customer with id {idCustomer} was not found");
        await _repository.Update<Customer>(customerToUpdate);
        return new CustomerDto.Response();
    }

    public async Task Delete(string id)
    {
        if (string.IsNullOrEmpty(id) || !(Guid.TryParse(id, out Guid idCustomer)))
            throw new FormatInvalidException("The id is invalid");
        var customerToDelete = await _repository.GetForId<Customer>(idCustomer);
        if (customerToDelete == null) throw new EntityNotFoundException($"The customer with id {idCustomer} was not found");
        await _repository.Delete<Customer>(customerToDelete);
    }
}