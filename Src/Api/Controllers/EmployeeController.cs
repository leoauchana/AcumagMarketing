using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EmployeeDto.Request employeeDto)
    {
        var employeeCreated = await _employeeService.Create(employeeDto);
        return Ok(employeeCreated);
    }

    [HttpGet("getAll")]
    public async Task<IActionResult> GetAll()
    {
        var employees = await _employeeService.GetAllEmployees();
        return Ok(employees);
    }

    [HttpGet("getById/{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var employeeFound = await _employeeService.GetEmployeeById(id);
        return Ok(employeeFound);
    }

    [HttpPatch]
    public async Task<IActionResult> Update([FromBody] EmployeeDto.RequestUpdate employeeDto)
    {
        var employeeUpdated = await _employeeService.Update(employeeDto);
        return Ok(employeeUpdated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _employeeService.GetAllEmployees();
        return Ok("Employee successfully deleted");
    }
}