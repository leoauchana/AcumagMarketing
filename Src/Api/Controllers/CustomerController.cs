using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly ICustomersService _customers;
    public CustomerController(ICustomersService customers)
    {
        _customers = customers;
    }

    [HttpGet("getAll")]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _customers.GetAllCustomers();
        return Ok(new
        {
            customers = customers,
            count = customers.Count()
        });
    }
    [HttpGet("getById/{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var customer = await _customers.GetCustomerById(id);
        return Ok(new { customerFound = customer });
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CustomerDto.Request customerDto)
    {
        var customer = await _customers.Create(customerDto);
        return Ok(new { newCustomer = customer });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _customers.Delete(id);
        return Ok("The customer was successfully deleted.");
    }
    [HttpPatch]
    public async Task<IActionResult> Update([FromBody] CustomerDto.RequestUpdate customerDto)
    {
        var customer = await _customers.Update(customerDto);
        return Ok(new { customerUpdated = customer });
    }
}