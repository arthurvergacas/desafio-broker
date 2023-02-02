using DesafioBroker.Brapi.Dtos;

namespace DesafioBroker.Brapi.Interfaces;

public interface IBrapiClient
{
    Task<StocksQuotesList> GetStocksQuotesList(string parsedTickers);
}
