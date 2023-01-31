using DesafioBroker.Brapi.Services;
using DesafioBroker.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DesafioBroker;

public class DesafioBroker
{
    public static async Task Main()
    {
        var host = HostManager.GetGenericHost();

        using (var serviceScope = host.Services.CreateScope())
        {
            var services = serviceScope.ServiceProvider;

            var configuration = services.GetService<ConfigurationService>()!.Configuration;

            Console.WriteLine(configuration.Stock.Brapi.QuotesUrl);

            var brapiService = services.GetService<BrapiService>()!;

            var tickers = new List<string> { "PETR4" };

            Console.WriteLine(await brapiService.GetTickersQuotesList(tickers));
        }
    }

}

