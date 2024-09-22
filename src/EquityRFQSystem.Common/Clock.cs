namespace EquityRFQSystem.Common;

public class Clock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}