using DesafioBroker.Brapi.Dtos;
using DesafioBroker.Brapi.Interfaces;

namespace DesafioBroker.Brapi.Services;

public class BrapiService
{
    private readonly IBrapiClient brapiClient;

    public BrapiService(IBrapiClient brapiClient)
    {
        this.brapiClient = brapiClient;
    }

    public async Task<TickersQuotesList> GetTickersQuotesList(IEnumerable<string> tickers)
    {
        ArgumentNullException.ThrowIfNull(tickers);

        if (!tickers.Any())
        {
            throw new ArgumentException("List of tickers cannot be empty", nameof(tickers));
        }

        return await this.brapiClient.GetTickersQuotesList(string.Join(',', tickers));
    }
}
