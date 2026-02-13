using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserDto.Request userDto)
    {
        var newUser = await _userService.Create(userDto);
        return Ok(newUser);
    }

    [HttpGet("/getById/{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var userFound = await _userService.GetById(id);
        return Ok(userFound);
    }

    [HttpGet("/getAll")]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAll();
        return Ok(users);
    }

    [HttpDelete("/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _userService.Delete(id);
        return Ok();
    }

    [HttpPatch]
    public async Task<IActionResult> Update([FromBody] UserDto.Request userDto)
    {
        var userUpdate = await _userService.Update(userDto);
        return Ok(userUpdate);
    }
}