using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuoteController : ControllerBase
{
    private readonly IQuoteService _quoteService;
    public QuoteController(IQuoteService quoteService)
    {
        _quoteService = quoteService;
    }
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] QuoteDto.Request newQuote)
    {
        var quote = await _quoteService.Register(newQuote);
        return Ok(quote);
    }
}