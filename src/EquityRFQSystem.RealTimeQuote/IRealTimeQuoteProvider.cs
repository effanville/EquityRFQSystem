namespace EquityRFQSystem.RealTimeQuote;

public interface IRealTimeQuoteProvider
{
    Task<Quote> GetCurrentMarketData(string ticker);
}
