using EquityRFQSystem.Models;

namespace EquityRFQSystem.Services;

public interface IQuoteRetrieverService
{
    Task<Quote?> GetTentativeQuote(string ticker, double? quantity);

    Task<Trade?> ConfirmTentativeQuote(long id, double? qty);

    Task<bool> CancelPendingQuote(long id);
}