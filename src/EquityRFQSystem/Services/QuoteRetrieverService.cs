using EquityRFQSystem.Common;
using EquityRFQSystem.Models;
using EquityRFQSystem.Persistence;
using EquityRFQSystem.RealTimeQuote;
using Quote = EquityRFQSystem.Models.Quote;

namespace EquityRFQSystem.Services;

public class QuoteRetrieverService : IQuoteRetrieverService
{
    /// <summary>
    /// This is the timeout after which the previously retrieved quote is considered
    /// to be invalid.
    /// </summary>
    private readonly int _staleQuoteTimeout = 300;

    private readonly IClock _clock;
    private readonly IPersistence _context;
    private readonly IRealTimeQuoteProvider _quoteProvider;

    public QuoteRetrieverService(
        IClock clock,
        IPersistence context,
        IRealTimeQuoteProvider quoteProvider)
    {
        _clock = clock;
        _context = context;
        _quoteProvider = quoteProvider;
    }

    public async Task<Quote?> GetTentativeQuote(string ticker, double? quantity)
    {
        if (string.IsNullOrEmpty(ticker))
        {
            return null;
        }

        if (string.IsNullOrEmpty(ticker))
        {
            return null;
        }

        RealTimeQuote.Quote quote = await _quoteProvider.GetCurrentMarketData(ticker);

        Persistence.Models.Quote providedQuote = new Persistence.Models.Quote()
        {
            TimeStamp = quote.Time,
            AskPrice = quote.AskPrice,
            BidPrice = quote.BidPrice,
            AskSize = quote.AskSize,
            BidSize = quote.BidSize,
            RequestedQty = quantity,
            Ticker = ticker,
            IsLive = true
        };

        _context.AddQuote(providedQuote);
        await _context.SaveChangesAsync();
        return new Quote()
        {
            Id = providedQuote.Id,
            Ticker = ticker,
            TimeStamp = quote.Time,
            AskPrice = quote.AskPrice,
            BidPrice = quote.BidPrice,
            AskSize = quote.AskSize,
            BidSize = quote.BidSize,
        };
    }

    public async Task<Trade?> ConfirmTentativeQuote(long id, double? qty)
    {
        Persistence.Models.Quote? providedQuote = await _context.FindQuoteAsync(id);
        if (providedQuote == null || providedQuote.IsFirm || !providedQuote.IsLive)
        {
            return null;
        }

        if (_clock.UtcNow - providedQuote.TimeStamp > TimeSpan.FromSeconds(_staleQuoteTimeout))
        {
            providedQuote.IsLive = false;
            await _context.SaveChangesAsync();
            return null;
        }

        double? quantity = qty ?? providedQuote.RequestedQty;
        if (!quantity.HasValue || quantity == 0)
        {
            return null;
        }

        bool isBuy = quantity > 0;
        double price = isBuy ? providedQuote.AskPrice : providedQuote.BidPrice;

        double availableQty = isBuy ? providedQuote.AskSize : -providedQuote.BidSize;

        double bookedQty = Math.Abs(availableQty) < Math.Abs(quantity.Value) ? availableQty : quantity.Value;

        Trade trade = new Trade()
        {
            TimeStamp = _clock.UtcNow,
            Ticker = providedQuote.Ticker,
            Quantity = bookedQty,
            Price = price
        };
        Persistence.Models.Trade modelTrade = new Persistence.Models.Trade()
        {
            TimeStamp = trade.TimeStamp,
            Ticker = trade.Ticker,
            Quantity = trade.Quantity,
            Price = price
        };

        providedQuote.IsLive = false;
        providedQuote.IsFirm = true;
        _context.BookTrade(modelTrade);
        await _context.SaveChangesAsync();
        return trade;
    }

    public async Task<bool> CancelPendingQuote(long id)
    {
        Persistence.Models.Quote? providedQuote = await _context.FindQuoteAsync(id);
        if (providedQuote == null)
        {
            return false;
        }

        providedQuote.IsLive = false;
        await _context.SaveChangesAsync();
        return true;
    }
}