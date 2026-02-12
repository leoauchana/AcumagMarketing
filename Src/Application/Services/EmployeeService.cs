using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using Transversal.Interfaces;

namespace Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IRepository _repository;
    private readonly IPasswordHasher _passwordHasher;
    public EmployeeService(IRepository repository, IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }

public async Task<IEnumerable<EmployeeDto.Response>> GetAllEmployees()
    {
        var employees = await _repository.GetAll<Employee>();
        if (!(employees.Count > 0)) return [];
        return employees.Select(c => new EmployeeDto.Response("","","","")).ToList();
    }

    public async Task<EmployeeDto.Response?> GetEmployeeById(string id)
    {
        if (string.IsNullOrEmpty(id) || !(Guid.TryParse(id, out Guid idEmployee)))
            throw new FormatInvalidException("The id is invalid");
        var employee = await _repository.GetForId<Customer>(idEmployee);
        if (employee == null) throw new EntityNotFoundException($"The customer with id {idEmployee} was not found");
        return new EmployeeDto.Response("", "", "", "");
    }

    public async Task<EmployeeDto.Response?> Create(EmployeeDto.Request employeeDto)
    {
        // var newEmployee = new Customer(customerDto.firstName, customerDto.lastName, Email.Create(customerDto.email),
        //     Dni.Create(customerDto.dni), new Domicilie(customerDto.street, customerDto.city,
        //         customerDto.number, customerDto.zipCode), customerDto.phoneNumber);
        // await _repository.Add(newEmployee);
        return new EmployeeDto.Response("", "", "", "");
    }

    public async Task<EmployeeDto.Response?> Update(EmployeeDto.RequestUpdate employeeDto)
    {
        // if (string.IsNullOrEmpty(employeeDto.id) || !(Guid.TryParse(employeeDto.id, out Guid idCustomer)))
        //     throw new FormatInvalidException("The id is invalid");
        // var employeeToUpdate = await _repository.GetForId<Customer>(idEmployee);
        // if (employeeToUpdate == null) throw new EntityNotFoundException($"The customer with id {idCustomer} was not found");
        // await _repository.Update<Customer>(employeeToUpdate);
        return new EmployeeDto.Response("", "", "", "");
    }

    public async Task Delete(string id)
    {
        if (string.IsNullOrEmpty(id) || !(Guid.TryParse(id, out Guid idEmployee)))
            throw new FormatInvalidException("The id is invalid");
        var employeeToDelete = await _repository.GetForId<Employee>(idEmployee);
        if (employeeToDelete == null) throw new EntityNotFoundException($"The customer with id {idEmployee} was not found");
        await _repository.Delete<Employee>(employeeToDelete);
    }
}