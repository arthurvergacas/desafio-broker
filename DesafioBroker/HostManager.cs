using DesafioBroker.Brapi.Clients;
using DesafioBroker.Brapi.Interfaces;
using DesafioBroker.Brapi.Services;
using DesafioBroker.Configuration;
using DesafioBroker.Mail.Interfaces;
using DesafioBroker.Mail.Services;
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
                    services.AddHttpClient<IBrapiClient, BrapiClient>();
                    services.AddTransient<ConfigurationService>();
                    services.AddTransient<BrapiService>();
                    services.AddTransient<IMailService, MailService>();
                })
                .UseConsoleLifetime();

            hostInstance = builder.Build();
        }

        return hostInstance;
    }

}
