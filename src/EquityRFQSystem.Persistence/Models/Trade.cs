namespace EquityRFQSystem.Persistence.Models;

public class Trade
{
    public long Id { get; set; }
    public required string Ticker { get; set; }
    public DateTime TimeStamp { get; set; }
    public double Price { get; set; }
    public double Quantity { get; set; }
}
