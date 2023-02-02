using DesafioBroker.Brapi.Dtos;

namespace DesafioBroker.Brapi.Interfaces;

public interface IBrapiService
{
    Task<StocksQuotesListDto> GetStocksQuotesList(IEnumerable<string> tickers);
}
