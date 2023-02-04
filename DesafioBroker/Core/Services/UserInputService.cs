using DesafioBroker.Core.Interfaces;
using DesafioBroker.Dtos;
using DesafioBroker.StockSubscription.Dtos;
using DesafioBroker.Misc;
using Microsoft.Extensions.Hosting;

namespace DesafioBroker.Core.Services;

public class UserInputService : IUserInputService
{
    private readonly IHostApplicationLifetime applicationLifetime;

    private bool applicationRunning = true;

    public UserInputService(IHostApplicationLifetime applicationLifetime)
    {
        this.applicationLifetime = applicationLifetime;

        this.applicationLifetime.ApplicationStopping.Register(this.OnApplicationStopping);
    }

    public StockSubscriptionDto ParseUserInput(string[] args)
    {
        if (args.Length != 3)
        {
            throw new ArgumentException(
                "Arguments not passed correctly. The syntax is '<PROGRAM> <TICKER> <PURCHASE_REFERENCE> <SALE_REFERENCE>'"
            );
        }

        var ticker = args[0].Trim();

        decimal purchaseReferenceValue;
        decimal saleReferenceValue;

        try
        {
            purchaseReferenceValue = decimal.Parse(
                args[1].Trim().Replace(',', '.'),
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.CurrentCulture
            );
            saleReferenceValue = decimal.Parse(
                args[2].Trim().Replace(',', '.'),
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.CurrentCulture
            );
        }
        catch
        {
            throw new ArgumentException("Unable to parse reference values to a number");
        }

        return new StockSubscriptionDto()
        {
            Ticker = ticker,
            StockReferenceValues = new StockReferenceValuesDto(purchaseReferenceValue, saleReferenceValue)
        };
    }

    public void WaitForUserCommands()
    {
        var command = string.Empty;
        while (this.applicationRunning && command != Commands.EXIT)
        {
            if (Console.KeyAvailable)
            {
                command = Console.ReadKey().KeyChar.ToString();
            }
        }
    }

    private void OnApplicationStopping()
    {
        this.applicationRunning = false;
    }
}
