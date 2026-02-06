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
    
}