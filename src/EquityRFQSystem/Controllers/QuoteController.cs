using EquityRFQSystem.Models;
using Microsoft.AspNetCore.Mvc;
using EquityRFQSystem.Services;

namespace EquityRFQSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuoteController : ControllerBase
{
    private readonly ILogger<QuoteController> _logger;
    private readonly IQuoteRetrieverService _quoteRetrieverService;

    public QuoteController(ILogger<QuoteController> logger, IQuoteRetrieverService quoteRetrieverService)
    {
        _logger = logger;
        _quoteRetrieverService = quoteRetrieverService;
    }

    [HttpGet(Name = "GetQuote")]
    public async Task<ActionResult<Quote>> Get(string ticker, double? quantity = null)
    {
        _logger.LogInformation($"Retrieved request for quote for Instrument='{ticker}', Qty='{quantity}'");

        if (string.IsNullOrEmpty(ticker))
            return BadRequest("Empty instrument identifier");

        var quote = await _quoteRetrieverService.GetTentativeQuote(ticker, quantity);

        if (quote == null)
        {
            return NotFound("Cannot find quote object");
        }

        return quote;
    }

    [HttpPost]
    public async Task<ActionResult<Trade>> ConfirmQuote(long id, double? quantity = null)
    {
        _logger.LogInformation($"Retrieved request for firming quote for Id='{id}', Qty='{quantity}'");
        var trade = await _quoteRetrieverService.ConfirmTentativeQuote(id, quantity);

        if (trade == null)
        {
            return NotFound("Error when booking trade");
        }

        return trade;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> CancelPendingQuote(long id)
    {
        _logger.LogInformation($"Retrieved cancel request for quote for Id='{id}'");
        bool cancelled = await _quoteRetrieverService.CancelPendingQuote(id);
        if (!cancelled)
        {
            return NotFound();
        }

        return NoContent();
    }
}
