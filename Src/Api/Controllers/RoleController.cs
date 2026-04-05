using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;
    public RoleController(IRoleService userService)
    {
        _roleService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RoleDto.Request roleDto)
    {
        var newRole = await _roleService.Create(roleDto);
        return Ok(new { newRole = newRole });
    }

    [HttpGet("getAll")]
    public async Task<IActionResult> GetAll()
    {
        var roles = await _roleService.GetAll();
        return Ok(new
        {
            roles = roles,
            count = roles.Count
        });
    }

    [HttpGet("getById/{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        // The service is not implemented
        var roleFound = await _roleService.GetById(id);
        return Ok(new { roleFound });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        // The service is not implemented
        await _roleService.Delete(id);
        return Ok(new
        {
            response = "The role was deleted successffuly"
        });
    }

    [HttpPatch]
    public async Task<IActionResult> Update([FromBody] RoleDto.Request roleDto)
    {
        // The service is not implemented
        var roleUpdated = await _roleService.Update(roleDto);
        return Ok(new { roleUpdated });
    }
}