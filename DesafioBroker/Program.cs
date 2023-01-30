using DesafioBroker.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DesafioBroker;

public class DesafioBroker
{
    public static void Main()
    {
        var host = HostManager.GetGenericHost();

        using (var serviceScope = host.Services.CreateScope())
        {
            var services = serviceScope.ServiceProvider;

            var configuration = services.GetService<ConfigurationService>()?.Configuration;

            Console.WriteLine(configuration?.Stock.Brapi.QuotesUrl);
        }
    }

}

