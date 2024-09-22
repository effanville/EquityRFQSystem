namespace EquityRFQSystem.RealTimeQuote;

public sealed class Quote
{
    public DateTime Time { get; set; }
    public required string Ticker { get; set; }
    public double BidPrice { get; set; }
    public double BidSize { get; set; }
    public double AskPrice { get; set; }
    public double AskSize { get; set; }
}
