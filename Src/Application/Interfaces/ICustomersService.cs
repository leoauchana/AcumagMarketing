using Application.DTOs;

namespace Application.Interfaces;

public interface ICustomersService
{
    Task<IEnumerable<CustomerDto.Response>> GetAllCustomers();
    Task<CustomerDto.Response?>  GetCustomerById(string id);
    Task<CustomerDto.Response?> Create(CustomerDto.Request customerDto);
    Task<CustomerDto.Response?> Update(CustomerDto.Request customerDto);
    Task Delete(string id);
}