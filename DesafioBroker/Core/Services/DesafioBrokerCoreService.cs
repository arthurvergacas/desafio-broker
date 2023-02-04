using DesafioBroker.Core.Interfaces;
using DesafioBroker.StockSubscription.Interfaces;
using DesafioBroker.Configuration.Interfaces;

namespace DesafioBroker.Core.Services;

public class DesafioBrokerCoreService : IDesafioBrokerCoreService
{
    private readonly UserInteractionService userInteractionService;

    private readonly IStockSubscriptionService stockSubscriptionService;

    private readonly IConfigurationService configurationService;

    private readonly IErrorHandlerService errorHandlerService;

    public DesafioBrokerCoreService(
        UserInteractionService userInteractionService,
        IStockSubscriptionService stockSubscriptionService,
        IErrorHandlerService errorHandlerService,
        IConfigurationService configurationService
    )
    {
        this.userInteractionService = userInteractionService;
        this.stockSubscriptionService = stockSubscriptionService;
        this.errorHandlerService = errorHandlerService;
        this.configurationService = configurationService;

        if (!this.configurationService.IsDevelopment())
        {
            AppDomain.CurrentDomain.UnhandledException += this.UnhandledExceptionTrapper;
        }
    }

    public void Run(string[] args)
    {
        this.ShowGreetingMessage();

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

    private void ShowGreetingMessage()
    {
        Console.WriteLine("Welcome to your personal stock tracker!\n");
        Console.WriteLine(
            $"The notifications about rise and fall of the chosen stock will be sent to {this.configurationService.Configuration.Email.Recipient}."
        );
        Console.WriteLine("\nAt any moment, you can terminate the tracker pressing 'q' on your keyboard.\n");
    }

    private void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
    {
        this.errorHandlerService.HandleError((Exception)e.ExceptionObject);
    }
}
