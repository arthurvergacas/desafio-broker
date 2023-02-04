using DesafioBroker.Brapi.Dtos;
using DesafioBroker.Brapi.Interfaces;
using DesafioBroker.Configuration.Interfaces;
using Newtonsoft.Json;

namespace DesafioBroker.Brapi.Clients;

public class BrapiClient : IBrapiClient
{
    private readonly HttpClient client;

    private readonly IConfigurationService configurationService;

    public BrapiClient(HttpClient client, IConfigurationService configurationService)
    {
        this.configurationService = configurationService;
        this.client = client;
        this.client.BaseAddress = new Uri(this.configurationService.Configuration!.Stock.Brapi.QuotesUrl);
    }

    public async Task<StocksQuotesListDto> GetStocksQuotesList(string parsedTickers)
    {
        var response = (await this.client.GetAsync(parsedTickers)).EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();
        using var streamReader = new StreamReader(stream, System.Text.Encoding.UTF8);
        using var jsonTextReader = new JsonTextReader(streamReader);

        return new JsonSerializer().Deserialize<StocksQuotesListDto>(jsonTextReader) ?? new StocksQuotesListDto();
    }

}
