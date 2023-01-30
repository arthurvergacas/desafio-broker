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
        return await this.brapiClient.GetTickersQuotesList(string.Join(',', tickers));
    }
}
