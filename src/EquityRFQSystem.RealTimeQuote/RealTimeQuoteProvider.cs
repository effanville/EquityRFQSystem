using EquityRFQSystem.Common;

namespace EquityRFQSystem.RealTimeQuote;

public sealed class RealTimeQuoteProvider : IRealTimeQuoteProvider
{
    private readonly IClock _clock;
    private readonly Random _random;

    public RealTimeQuoteProvider(IClock clock, Random random)
    {
        _clock = clock;
        _random = random;
    }

    public async Task<Quote> GetCurrentMarketData(string ticker)
    {
        await Task.Delay(1);
        double bidPrice = _random.Next(0, 255);
        return new Quote()
        {
            Ticker = ticker,
            Time = _clock.UtcNow,
            BidPrice = bidPrice,
            AskPrice = bidPrice + _random.Next(0, 5),
            BidSize = _random.Next(0, 25),
            AskSize = _random.Next(0, 33)
        };
    }
}
