using DesafioBroker.Brapi.Dtos;
using DesafioBroker.Brapi.Interfaces;

namespace DesafioBroker.Brapi.Services;

public class BrapiService
{
    private readonly IHttpClientFactory clientFactory;

    public BrapiService(IHttpClientFactory clientFactory)
    {
        this.clientFactory = clientFactory;
    }

    public async Task<TickersQuotesList> GetTickersQuotesList()
    {
        throw new NotImplementedException();
    }
}
