using DesafioBroker.Brapi.Clients;
using DesafioBroker.Brapi.Interfaces;
using DesafioBroker.Brapi.Services;
using DesafioBroker.Configuration.Interfaces;
using DesafioBroker.Configuration.Services;
using DesafioBroker.Mail.Interfaces;
using DesafioBroker.Mail.Services;
using DesafioBroker.StockSubscription.Interfaces;
using DesafioBroker.StockSubscription.Services;
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
                    services.AddTransient<IConfigurationService, ConfigurationService>();
                    services.AddTransient<IBrapiService, BrapiService>();
                    services.AddTransient<IMailService, MailService>();
                    services.AddTransient<IMailMessageService, MailMessageService>();
                    services.AddTransient<IStockSubscriptionService, StockSubscriptionService>();
                })
                .UseConsoleLifetime();

            hostInstance = builder.Build();
        }

        return hostInstance;
    }

}
