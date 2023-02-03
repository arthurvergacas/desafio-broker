using DesafioBroker.Core.Interfaces;
using DesafioBroker.StockSubscription.Interfaces;

namespace DesafioBroker.Core.Services;

public class DesafioBrokerCoreService : IDesafioBrokerCoreService
{
    private readonly IUserInputService userInputService;

    private readonly IStockSubscriptionService stockSubscriptionService;

    public DesafioBrokerCoreService(IUserInputService userInputService, IStockSubscriptionService stockSubscriptionService)
    {
        this.userInputService = userInputService;
        this.stockSubscriptionService = stockSubscriptionService;
    }

    public void Run(string[] args)
    {
        var stockSubscription = this.userInputService.ParseUserInput(args);
        this.stockSubscriptionService.SubscribeToStock(stockSubscription);

        this.userInputService.WaitForUserCommands();

        this.stockSubscriptionService.Dispose();
    }
}
