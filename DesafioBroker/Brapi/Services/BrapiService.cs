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
        try
        {
            return await this.brapiClient.GetTickersQuotesList(tickers);
        }
        catch
        {
            // TODO receber os erros de não encontrado ou erro de api e essas coisas que vc vai lançar no client
            return new TickersQuotesList();
        }

    }
}
