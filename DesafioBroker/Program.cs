using System.Net.Mail;
using Microsoft.Extensions.DependencyInjection;
using DesafioBroker.Mail.Interfaces;
using DesafioBroker.Configuration.Interfaces;
using DesafioBroker.Brapi.Interfaces;
using DesafioBroker.Brapi.Dtos;
using DesafioBroker.Dtos;

namespace DesafioBroker;

public class DesafioBroker
{
    public static async Task Main()
    {
        var host = HostManager.GetGenericHost();

        using (var serviceScope = host.Services.CreateScope())
        {
            var services = serviceScope.ServiceProvider;

            var configuration = services.GetService<IConfigurationService>()!.Configuration;

            Console.WriteLine(configuration.Stock.Brapi.QuotesUrl);

            var brapiService = services.GetService<IBrapiService>()!;

            var tickers = new List<string> { "PETR4" };

            var quotesList = await brapiService.GetStocksQuotesList(tickers);
            Console.WriteLine(quotesList + "\n\n");

            var mailMessageService = services.GetService<IMailMessageService>()!;
            var mail = mailMessageService.CreateNotificationMessage(
                new StockReferenceValuesDto(25.2312m, 10),
                quotesList.Results[0]
            );

            //envia a mensagem
            var mailService = services.GetService<IMailService>()!;
            mailService.SendMail(mail);
        }
    }

}

