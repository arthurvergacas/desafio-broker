using DesafioBroker.Brapi.Dtos;
using DesafioBroker.Brapi.Interfaces;
using DesafioBroker.Configuration;
using Newtonsoft.Json;

namespace DesafioBroker.Brapi.Clients;

public class BrapiClient : IBrapiClient
{
    private readonly HttpClient client;

    private readonly ConfigurationService configurationService;

    public BrapiClient(HttpClient client, ConfigurationService configurationService)
    {
        this.configurationService = configurationService;
        this.client = client;
        this.client.BaseAddress = new Uri(this.configurationService.Configuration!.Stock.Brapi.QuotesUrl);
    }

    public async Task<TickersQuotesList?> GetTickersQuotesList(IEnumerable<string> tickers)
    {
        var response = await this.client.GetAsync($"/{string.Join(',', tickers)}");

        if (response.IsSuccessStatusCode)
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            using var streamReader = new StreamReader(stream);
            using var jsonTextReader = new JsonTextReader(streamReader);

            return new JsonSerializer().Deserialize<TickersQuotesList>(jsonTextReader);
        }

        return new TickersQuotesList();
    }

}
