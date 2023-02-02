using DesafioBroker.StockSubscription.Interfaces;
using DesafioBroker.StockSubscription.Dtos;
using DesafioBroker.Mail.Interfaces;
using DesafioBroker.Brapi.Interfaces;
using DesafioBroker.Configuration.Interfaces;
using DesafioBroker.Dtos;
using System.Timers;
using Timer = System.Timers.Timer;
using DesafioBroker.Brapi.Dtos;

namespace DesafioBroker.StockSubscription.Services;

public class StockSubscriptionService : IStockSubscriptionService
{

    private enum NotificationType
    {
        SALE,
        PURCHASE,
        NONE
    }

    private readonly IBrapiService brapiService;

    private readonly IMailService mailService;

    private readonly IMailMessageService mailMessageService;

    private readonly IConfigurationService configurationService;

    private readonly Timer notificationTimer;

    private StockSubscriptionDto stockSubscription = null!;

    private NotificationType lastNotificationSentType = NotificationType.NONE;

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

        this.notificationTimer = this.CreateTimer();
    }

    ~StockSubscriptionService()
    {
        this.notificationTimer.Stop();
        this.notificationTimer.Dispose();
    }

    public void SubscribeToStock(string ticker, StockReferenceValuesDto stockReferenceValues)
    {
        this.stockSubscription = new StockSubscriptionDto()
        {
            Ticker = ticker,
            StockReferenceValues = stockReferenceValues
        };

        this.notificationTimer.Elapsed += this.OnNotificationTimerEvent;
        this.notificationTimer.Start();
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
        var stockQuotes = await this.brapiService.GetStocksQuotesList(new List<string>() { this.stockSubscription.Ticker });

        return stockQuotes.Results.First(quotes => quotes.Symbol == this.stockSubscription.Ticker);
    }

    public bool ShouldNotifyUser(StockQuotesDto stockQuotes)
    {
        return this.ShouldSendSaleNotification(stockQuotes) || this.ShouldSendPurchaseNotification(stockQuotes);
    }

    public bool ShouldSendSaleNotification(StockQuotesDto stockQuotes)
    {
        var isSaleScenario =
            this.stockSubscription.StockReferenceValues.SaleReferenceValue < stockQuotes.RegularMarketPrice;

        return isSaleScenario && !NotificationType.SALE.Equals(this.lastNotificationSentType);
    }

    public bool ShouldSendPurchaseNotification(StockQuotesDto stockQuotes)
    {
        var iPurchaseScenario =
            this.stockSubscription.StockReferenceValues.PurchaseReferenceValue > stockQuotes.RegularMarketPrice;

        return iPurchaseScenario && !NotificationType.PURCHASE.Equals(this.lastNotificationSentType);
    }

    private void NotifyUser(StockQuotesDto stockQuotes)
    {
        var mailMessage = this.mailMessageService.CreateNotificationMessage(
            this.stockSubscription.StockReferenceValues,
            stockQuotes
        );

        this.mailService.SendMail(mailMessage);

        if (this.ShouldSendSaleNotification(stockQuotes))
        {
            this.lastNotificationSentType = NotificationType.SALE;
        }
        else if (this.ShouldSendPurchaseNotification(stockQuotes))
        {
            this.lastNotificationSentType = NotificationType.PURCHASE;
        }
        else
        {
            this.lastNotificationSentType = NotificationType.NONE;
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
