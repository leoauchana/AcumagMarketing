using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using Application.Shared;
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
        if (!(customers.Any())) return [];
        return customers.Select(c => new CustomerDto.Response(c.Id.ToString(), c.FirstName, c.LastName, c.Email.Value, c.Dni.Value,
            c.Domicilie.City, c.Domicilie.Street, c.Domicilie.Number, c.Domicilie.ZipCode, c.PhoneNumber)).ToList();
    }

    public async Task<CustomerDto.Response?> GetCustomerById(string id)
    {
        var idCustomer = id.ValidateId();
        var customer = await _repository.GetForId<Customer>(idCustomer);
        if (customer == null) throw new EntityNotFoundException($"The customer with id {idCustomer} was not found");
        return new CustomerDto.Response(customer.Id.ToString(), customer.FirstName, customer.LastName, customer.Email.Value, customer.Dni.Value,
            customer.Domicilie.City, customer.Domicilie.Street, customer.Domicilie.Number, customer.Domicilie.ZipCode, customer.PhoneNumber);
    }

    public async Task<CustomerDto.Response?> Create(CustomerDto.Request customerDto)
    {
        var customerFound = await _repository.GetTheFirstOne<Customer>(c => c.Email.Value.Equals(customerDto.email) || c.Dni.Value.Equals(customerDto.dni));
        if(customerDto == null) throw new BusinessConflictException("The email or dni already exists");
        var newCustomer = new Customer(customerDto.firstName, customerDto.lastName, Email.Create(customerDto.email),
            Dni.Create(customerDto.dni), new Domicilie(customerDto.street, customerDto.city,
                customerDto.number, customerDto.zipCode), customerDto.phoneNumber);
        await _repository.Add(newCustomer);
        return new CustomerDto.Response(newCustomer.Id.ToString(), newCustomer.FirstName, newCustomer.LastName, newCustomer.Email.Value, newCustomer.Dni.Value,
            newCustomer.Domicilie.City, newCustomer.Domicilie.Street, newCustomer.Domicilie.Number, newCustomer.Domicilie.ZipCode, newCustomer.PhoneNumber);
    }

    public async Task<CustomerDto.Response?> Update(CustomerDto.RequestUpdate customerDto)
    {
        var idCustomer = customerDto.id.ValidateId();
        var customerToUpdate = await _repository.GetForId<Customer>(idCustomer);
        if (customerToUpdate == null) throw new EntityNotFoundException($"The customer with id {idCustomer} was not found");
        customerToUpdate.Update(customerDto.firstName, customerDto.lastName, customerDto.phoneNumber);
        await _repository.Update(customerToUpdate);
        return new CustomerDto.Response(customerToUpdate.Id.ToString(), customerToUpdate.FirstName, customerToUpdate.LastName, customerToUpdate.Email.Value,
            customerToUpdate.Dni.Value, customerToUpdate.Domicilie.City, customerToUpdate.Domicilie.Street, customerToUpdate.Domicilie.Number, 
            customerToUpdate.Domicilie.ZipCode, customerToUpdate.PhoneNumber);
    }

    public async Task Delete(string id)
    {
        var idCustomer = id.ValidateId();
        var customerToDelete = await _repository.GetForId<Customer>(idCustomer);
        if (customerToDelete == null) throw new EntityNotFoundException($"The customer with id {idCustomer} was not found");
        await _repository.Delete(customerToDelete);
    }
}