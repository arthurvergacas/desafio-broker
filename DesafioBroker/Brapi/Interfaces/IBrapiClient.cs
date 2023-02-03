using DesafioBroker.Brapi.Dtos;

namespace DesafioBroker.Brapi.Interfaces;

public interface IBrapiClient
{
    Task<StocksQuotesListDto> GetStocksQuotesList(string parsedTickers);
}
