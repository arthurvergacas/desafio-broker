using DesafioBroker.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DesafioBroker;

internal sealed class HostManager
{

    private static IHost? hostInstance;

    private HostManager() { }

    public static IHost GetGenericHost()
    {

        if (hostInstance == null)
        {
            var builder = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<ConfigurationService>();
                })
                .UseConsoleLifetime();

            hostInstance = builder.Build();
        }

        return hostInstance;
    }

}
