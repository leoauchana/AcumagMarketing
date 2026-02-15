using Application.DTOs;
using Application.Interfaces;
using Domain.Interfaces;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IRepository _repository;
    public UserService(IRepository repository)
    {
        _repository = repository;
    }

    public Task<UserDto.Response> GetById(string id)
    {
        throw new NotImplementedException();
    }
    public Task<List<UserDto.Response>> GetAll()
    {
        throw new NotImplementedException();
    }
    public Task<UserDto.Response> Create(UserDto.Request userDto)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto.Response> Update(UserDto.Request userDto)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto.Response> Delete(string id)
    {
        throw new NotImplementedException();
    }
}