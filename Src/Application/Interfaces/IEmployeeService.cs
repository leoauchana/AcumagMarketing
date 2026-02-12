using Application.DTOs;

namespace Application.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto.Response>> GetAllEmployees();
    Task<EmployeeDto.Response?>  GetEmployeeById(string id);
    Task<EmployeeDto.Response?> Create(EmployeeDto.Request employeeDto);
    Task<EmployeeDto.Response?> Update(EmployeeDto.RequestUpdate employeeDto);
    Task Delete(string id);
}