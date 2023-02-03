using DesafioBroker.Brapi.Dtos;
using DesafioBroker.Brapi.Interfaces;

namespace DesafioBroker.Brapi.Services;

public class BrapiService : IBrapiService
{
    private readonly IBrapiClient brapiClient;

    public BrapiService(IBrapiClient brapiClient)
    {
        this.brapiClient = brapiClient;
    }

    public async Task<StocksQuotesListDto> GetStocksQuotesList(IEnumerable<string> tickers)
    {
        ArgumentNullException.ThrowIfNull(tickers, nameof(tickers));

        if (!tickers.Any())
        {
            throw new ArgumentException("List of tickers cannot be empty", nameof(tickers));
        }

        return await this.brapiClient.GetStocksQuotesList(string.Join(',', tickers));
    }
}
