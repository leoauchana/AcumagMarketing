using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using Application.Shared;
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

    public async Task<List<EmployeeDto.Response>> GetAllEmployees()
    {
        var employees = await _repository.ListAll<Employee>(nameof(Role));
        if (!(employees.Count > 0)) return [];
        return employees.Select(e => new EmployeeDto.Response(e.Id.ToString(), e.FirstName, e.LastName, e.Email.Value, e.Dni.Value,
            e.Domicilie.City, e.Domicilie.Street, e.Domicilie.Number, e.Role.Name)).ToList();
    }

    public async Task<EmployeeDto.Response?> GetEmployeeById(string id)
    {
        var idEmployee = id.ValidateId();
        var employee = await _repository.GetForId<Employee>(idEmployee, nameof(Role));
        if (employee == null) throw new EntityNotFoundException($"The customer with id {idEmployee} was not found");
        return new EmployeeDto.Response(employee.Id.ToString(), employee.FirstName, employee.LastName, employee.Email.Value, employee.Dni.Value,
            employee.Domicilie.City, employee.Domicilie.Street, employee.Domicilie.Number, employee.Role.Name);
    }

    public async Task<EmployeeDto.Response?> Create(EmployeeDto.Request employeeDto)
    {
        var email = Email.Create(employeeDto.email);
        var dni = Dni.Create(employeeDto.dni);
        var employeeFound = await _repository.GetTheFirstOne<Employee>(e => e.Email == email || e.Dni == dni);
        if (employeeFound != null) throw new BusinessConflictException("The email or dni already exists");
        var idRolee = employeeDto.idRole.ValidateId();
        var roleFound = await _repository.GetForId<Role>(idRolee);
        if (roleFound == null) throw new BusinessConflictException("The role is not exists");
        var passhordHash = _passwordHasher.Hash(employeeDto.password);
        var newEmployee = new Employee(employeeDto.firstName, employeeDto.lastName, Email.Create(employeeDto.email),
            Dni.Create(employeeDto.dni), new Domicilie(employeeDto.street, employeeDto.city,
                employeeDto.number, employeeDto.zipCode), roleFound, new User(employeeDto.userName, passhordHash));
        await _repository.Add(newEmployee);
        return new EmployeeDto.Response(newEmployee.Id.ToString(), newEmployee.FirstName, newEmployee.LastName, newEmployee.Email.Value, newEmployee.Dni.Value,
            newEmployee.Domicilie.City, newEmployee.Domicilie.Street, newEmployee.Domicilie.Number, newEmployee.Role.Name);
    }

    public async Task<EmployeeDto.Response?> Update(EmployeeDto.RequestUpdate employeeDto)
    {
        var idEmployee = employeeDto.id.ValidateId();
        var employeeToUpdate = await _repository.GetForId<Employee>(idEmployee, nameof(Role));
        if (employeeToUpdate == null) throw new EntityNotFoundException($"The employee with id {idEmployee} was not found");
        if (!(Guid.TryParse(employeeDto.idRole, out var idRolee))) throw new FormatInvalidException("The id is invalid");
        var roleFound = await _repository.GetForId<Role>(idRolee);
        if (roleFound == null) throw new BusinessConflictException("The role is not exists");
        employeeToUpdate.UpdateRole(roleFound);
        await _repository.Update(employeeToUpdate);
        return new EmployeeDto.Response(employeeToUpdate.Id.ToString(), employeeToUpdate.FirstName, employeeToUpdate.LastName, employeeToUpdate.Email.Value, employeeToUpdate.Dni.Value,
            employeeToUpdate.Domicilie.City, employeeToUpdate.Domicilie.Street, employeeToUpdate.Domicilie.Number, employeeToUpdate.Role.Name);
    }

    public async Task Delete(string id)
    {
        var idEmployee = id.ValidateId();
        var employeeToDelete = await _repository.GetForId<Employee>(idEmployee);
        if (employeeToDelete == null) throw new EntityNotFoundException($"The customer with id {idEmployee} was not found");
        await _repository.Delete(employeeToDelete);
    }
}