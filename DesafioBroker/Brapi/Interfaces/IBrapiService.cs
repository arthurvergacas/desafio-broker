using DesafioBroker.Brapi.Dtos;

namespace DesafioBroker.Brapi.Interfaces;

public interface IBrapiService
{
    Task<StocksQuotesList> GetStocksQuotesList(IEnumerable<string> tickers);
}
