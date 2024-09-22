namespace EquityRFQSystem.Models;

public class Trade
{
    public DateTime TimeStamp { get; init; }
    public required string Ticker { get; init; }
    public double Price { get; init; }
    public double Quantity { get; init; }

}
