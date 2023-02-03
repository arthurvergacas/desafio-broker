using DesafioBroker.Core.Interfaces;
using DesafioBroker.Dtos;
using DesafioBroker.StockSubscription.Dtos;
using DesafioBroker.Misc;

namespace DesafioBroker.Core.Services;

public class UserInputService : IUserInputService
{
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
                args[1].Trim(),
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.CurrentCulture
            );
            saleReferenceValue = decimal.Parse(
                args[2].Trim(),
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
        while (command != Commands.EXIT)
        {
            command = Console.ReadKey().KeyChar.ToString();
        }
    }
}
