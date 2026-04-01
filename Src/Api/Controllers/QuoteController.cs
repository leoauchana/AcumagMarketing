using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
    public async Task<IActionResult> Register([FromForm] QuoteOrderDto.RequestFormFile newQuote)
    {
        var idUser = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        if (idUser == null) return BadRequest("User can not getter");
        var quoteDto = new QuoteOrderDto.RequestStream(newQuote.quoteFile.OpenReadStream(), newQuote.quoteFile.FileName, newQuote.quoteFile.ContentType
            , newQuote.idCustomer);
        var quote = await _quoteService.Register(quoteDto, idUser);
        return Ok(new { quoteRegistered = quote });
    }
}