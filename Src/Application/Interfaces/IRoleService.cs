using Application.DTOs;

namespace Application.Interfaces;

public interface IRoleService
{
    Task<RoleDto.Response> GetById(string id);
    Task<List<RoleDto.Response>> GetAll();
    Task<RoleDto.Response> Create(RoleDto.Request userDto);
    Task<RoleDto.Response> Update(RoleDto.Request userDto);
    Task<RoleDto.Response> Delete(string id);
}