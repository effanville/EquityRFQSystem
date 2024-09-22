namespace EquityRFQSystem.Persistence.Models;

public class Quote
{
    public long Id { get; set; }
    public required string Ticker { get; set; }
    public DateTime TimeStamp { get; set; }
    public bool IsFirm { get; set; }
    public bool IsLive { get; set; }
    public double BidPrice { get; set; }
    public double BidSize { get; set; }
    public double AskPrice { get; set; }
    public double AskSize { get; set; }
    public double? RequestedQty { get; set; }
}
