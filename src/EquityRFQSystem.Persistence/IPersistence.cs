using EquityRFQSystem.Persistence.Models;

namespace EquityRFQSystem.Persistence;

public interface IPersistence
{
    IQueryable<Quote> Quotes { get; }
    IQueryable<Trade> BookedTrades { get; }
    void AddQuote(Quote quote);
    void BookTrade(Trade trade);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    ValueTask<Quote?> FindQuoteAsync(params object?[]? keyValues);
}
