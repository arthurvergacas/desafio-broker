using DesafioBroker.StockSubscription.Interfaces;
using DesafioBroker.StockSubscription.Dtos;
using DesafioBroker.Mail.Interfaces;
using DesafioBroker.Brapi.Interfaces;
using DesafioBroker.Configuration.Interfaces;
using System.Timers;
using Timer = System.Timers.Timer;
using DesafioBroker.Brapi.Dtos;

namespace DesafioBroker.StockSubscription.Services;

public class StockSubscriptionService : IStockSubscriptionService
{

    public enum NotificationType
    {
        SALE,
        PURCHASE,
        NONE
    }

    private readonly IBrapiService brapiService;

    private readonly IMailService mailService;

    private readonly IMailMessageService mailMessageService;

    private readonly IConfigurationService configurationService;

    public Timer NotificationTimer { get; set; }

    public StockSubscriptionDto StockSubscription { get; set; } = null!;

    public NotificationType LastNotificationSentType { get; set; } = NotificationType.NONE;

    public StockSubscriptionService(
        IBrapiService brapiService,
        IMailService mailService,
        IMailMessageService mailMessageService,
        IConfigurationService configurationService
    )
    {
        this.brapiService = brapiService;
        this.mailService = mailService;
        this.mailMessageService = mailMessageService;
        this.configurationService = configurationService;

        this.NotificationTimer = this.CreateTimer();
    }

    ~StockSubscriptionService()
    {
        this.NotificationTimer.Stop();
        this.NotificationTimer.Dispose();
    }

    public void SubscribeToStock(StockSubscriptionDto stockSubscription)
    {
        this.StockSubscription = stockSubscription;

        this.NotificationTimer.Elapsed += this.OnNotificationTimerEvent;
        this.NotificationTimer.Start();
    }

    public async void OnNotificationTimerEvent(object? source, ElapsedEventArgs e)
    {
        var stockQuotes = await this.GetSubscribedStockQuotes();

        if (this.ShouldNotifyUser(stockQuotes))
        {
            this.NotifyUser(stockQuotes);
        }
    }

    public async Task<StockQuotesDto> GetSubscribedStockQuotes()
    {
        var stockQuotes =
            await this.brapiService.GetStocksQuotesList(new List<string>() { this.StockSubscription.Ticker });

        return stockQuotes.Results.First(quotes => quotes.Symbol == this.StockSubscription.Ticker);
    }

    public bool ShouldNotifyUser(StockQuotesDto stockQuotes)
    {
        return this.ShouldSendSaleNotification(stockQuotes) || this.ShouldSendPurchaseNotification(stockQuotes);
    }

    public bool ShouldSendSaleNotification(StockQuotesDto stockQuotes)
    {
        var isSaleScenario =
            this.StockSubscription.StockReferenceValues.SaleReferenceValue < stockQuotes.RegularMarketPrice;

        return isSaleScenario && !NotificationType.SALE.Equals(this.LastNotificationSentType);
    }

    public bool ShouldSendPurchaseNotification(StockQuotesDto stockQuotes)
    {
        var iPurchaseScenario =
            this.StockSubscription.StockReferenceValues.PurchaseReferenceValue > stockQuotes.RegularMarketPrice;

        return iPurchaseScenario && !NotificationType.PURCHASE.Equals(this.LastNotificationSentType);
    }

    public void NotifyUser(StockQuotesDto stockQuotes)
    {
        var mailMessage = this.mailMessageService.CreateNotificationMessage(
            this.StockSubscription.StockReferenceValues,
            stockQuotes
        );

        this.mailService.SendMail(mailMessage);

        if (this.ShouldSendSaleNotification(stockQuotes))
        {
            this.LastNotificationSentType = NotificationType.SALE;
        }
        else if (this.ShouldSendPurchaseNotification(stockQuotes))
        {
            this.LastNotificationSentType = NotificationType.PURCHASE;
        }
        else
        {
            this.LastNotificationSentType = NotificationType.NONE;
        }
    }

    public Timer CreateTimer()
    {
        return new Timer()
        {
            Interval = 1000 * this.configurationService.Configuration.Notification.FetchInterval, // every 5 minutes
            AutoReset = true,
            Enabled = false
        };
    }
}
