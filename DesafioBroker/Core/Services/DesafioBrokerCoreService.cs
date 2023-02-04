using DesafioBroker.Core.Interfaces;
using DesafioBroker.StockSubscription.Interfaces;

namespace DesafioBroker.Core.Services;

public class DesafioBrokerCoreService : IDesafioBrokerCoreService
{
    private readonly UserInteractionService userInteractionService;

    private readonly IStockSubscriptionService stockSubscriptionService;

    private readonly IErrorHandlerService errorHandlerService;

    public DesafioBrokerCoreService(UserInteractionService userInteractionService, IStockSubscriptionService stockSubscriptionService, IErrorHandlerService errorHandlerService)
    {
        this.userInteractionService = userInteractionService;
        this.stockSubscriptionService = stockSubscriptionService;
        this.errorHandlerService = errorHandlerService;
    }

    public void Run(string[] args)
    {
        try
        {
            var stockSubscription = this.userInteractionService.ParseUserInput(args);
            this.stockSubscriptionService.SubscribeToStock(stockSubscription);
        }
        catch (Exception e)
        {
            this.errorHandlerService.HandleError(e);
        }

        this.userInteractionService.WaitForUserCommands();
    }
}
