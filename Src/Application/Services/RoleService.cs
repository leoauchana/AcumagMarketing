using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Transversal.Interfaces;

namespace Application.Services;

public class RoleService : IRoleService
{
    private readonly IRepository _repository;
    public RoleService(IRepository repository)
    {
        _repository = repository;
    }

    Task<RoleDto.Response> IRoleService.GetById(string id)
    {
        throw new NotImplementedException();
    }

    async Task<List<RoleDto.Response>> IRoleService.GetAll()
    {
        var rolesFound = await _repository.GetAll<Role>();
        if (!rolesFound.Any()) return [];
        return rolesFound.Select(r => new RoleDto.Response(r.Id.ToString(), r.Name, r.Description)).ToList();
    }

    public async Task<RoleDto.Response> Create(RoleDto.Request roleDto)
    {
        var rolNameNormalized = roleDto.rolName.ToLower().Trim();
        var roleFound = await _repository.GetTheFirstOne<Role>(r => r.Name.ToLower() == rolNameNormalized);
        if (roleFound != null) throw new BusinessConflictException("The role already exists");
        var newRole = new Role(roleDto.rolName, roleDto.description);
        await _repository.Add<Role>(newRole);
        return new RoleDto.Response(newRole.Id.ToString(), newRole.Name, newRole.Description);
    }

    public Task<RoleDto.Response> Update(RoleDto.Request userDto)
    {
        throw new NotImplementedException();
    }

    Task<RoleDto.Response> IRoleService.Delete(string id)
    {
        throw new NotImplementedException();
    }
}