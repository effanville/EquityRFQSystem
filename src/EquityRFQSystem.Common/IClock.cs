namespace EquityRFQSystem.Common
{
    public interface IClock
    {
        DateTime UtcNow { get; }
    }
}
