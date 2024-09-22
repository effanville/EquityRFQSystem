using EquityRFQSystem.Persistence.Models;

namespace EquityRFQSystem.Persistence;

public class Persistence : IPersistence
{
    private readonly PersistenceContext _context;

    public Persistence(PersistenceContext context) 
        => _context = context;

    public IQueryable<Quote> Quotes => _context.Quotes;
    public IQueryable<Trade> BookedTrades => _context.BookedTrades;

    public void AddQuote(Quote quote)
        => _context.Quotes.Add(quote);

    public void BookTrade(Trade trade)
        => _context.BookedTrades.Add(trade);

    public ValueTask<Quote?> FindQuoteAsync(params object?[]? keyValues)
        => _context.Quotes.FindAsync(keyValues);
    

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) 
        => _context.SaveChangesAsync(cancellationToken);
}
