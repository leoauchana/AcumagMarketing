using Application.DTOs;
using Application.Interfaces;

namespace Application.Services;

public class CustomersService : ICustomersService
{
    public CustomersService()
    {
        
    }
    public Task<IEnumerable<CustomerDto.Response>> GetAllCustomers()
    {
        throw new NotImplementedException();
    }

    public Task<CustomerDto.Response?> GetCustomerById(string id)
    {
        throw new NotImplementedException();
    }

    public Task<CustomerDto.Response?> Create(CustomerDto.Request customerDto)
    {
        throw new NotImplementedException();
    }

    public Task<CustomerDto.Response?> Update(CustomerDto.Request customerDto)
    {
        throw new NotImplementedException();
    }

    public Task Delete(string id)
    {
        throw new NotImplementedException();
    }
}