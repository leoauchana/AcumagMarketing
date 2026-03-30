using Application.DTOs;

namespace Application.Interfaces;

public interface IUserService
{
    Task<UserDto.Response> GetById(string id);
    Task<List<UserDto.Response>> GetAll();
    Task<UserDto.Response> Create(UserDto.Request userDto);
    Task<UserDto.Response> Update(UserDto.Request userDto);
    Task<UserDto.Response> Delete(string id);
}