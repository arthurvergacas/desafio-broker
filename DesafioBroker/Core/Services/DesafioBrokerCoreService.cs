using DesafioBroker.Core.Interfaces;
using DesafioBroker.StockSubscription.Interfaces;

namespace DesafioBroker.Core.Services;

public class DesafioBrokerCoreService : IDesafioBrokerCoreService
{
    private readonly UserInteractionService userInteractionService;

    private readonly IStockSubscriptionService stockSubscriptionService;

    public DesafioBrokerCoreService(UserInteractionService userInputService, IStockSubscriptionService stockSubscriptionService)
    {
        this.userInteractionService = userInputService;
        this.stockSubscriptionService = stockSubscriptionService;
    }

    public void Run(string[] args)
    {
        var stockSubscription = this.userInteractionService.ParseUserInput(args);
        this.stockSubscriptionService.SubscribeToStock(stockSubscription);

        this.userInteractionService.WaitForUserCommands();
    }
}
