using DesafioBroker.Brapi.Dtos;

namespace DesafioBroker.Brapi.Interfaces;

public interface IBrapiClient
{
    Task<TickersQuotesList> GetTickersQuotesList(IEnumerable<string> tickers);
}
