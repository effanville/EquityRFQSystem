namespace EquityRFQSystem.Models;

public class Quote
{
    public long Id { get; init; }
    public required string Ticker { get; init; }
    public DateTime TimeStamp { get; init; }
    public double BidPrice { get; init; }
    public double BidSize { get; init; }
    public double AskPrice { get; init; }
    public double AskSize { get; init; }
}
