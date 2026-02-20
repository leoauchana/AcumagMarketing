using Application.DTOs;
using Application.Interfaces;
using Domain.Interfaces;
using Transversal.Interfaces;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IRepository _repository;
    private readonly IPasswordHasher _passwordHasher;
    public UserService(IRepository repository, IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
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