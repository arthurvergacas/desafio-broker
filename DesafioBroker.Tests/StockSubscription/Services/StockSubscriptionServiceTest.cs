using DesafioBroker.Tests.Fixtures;
using DesafioBroker.Brapi.Interfaces;
using DesafioBroker.Configuration.Interfaces;
using DesafioBroker.Mail.Interfaces;
using Moq;
using DesafioBroker.StockSubscription.Services;
using DesafioBroker.Dtos;
using DesafioBroker.StockSubscription.Dtos;
using System.Net.Mail;
using DesafioBroker.Brapi.Dtos;

namespace DesafioBroker.Tests.StockSubscription.Services;

public class StockSubscriptionServiceTest
{
    private readonly Mock<IBrapiService> mockBrapiService;

    private readonly Mock<IMailService> mockMailService;

    private readonly Mock<IMailMessageService> mockMailMessageService;

    private readonly Mock<IConfigurationService> mockConfigurationService;

    private readonly Configuration.Models.Configuration mockConfiguration;

    public StockSubscriptionServiceTest()
    {
        this.mockBrapiService = new Mock<IBrapiService>();

        this.mockMailService = new Mock<IMailService>();

        this.mockMailMessageService = new Mock<IMailMessageService>();

        this.mockConfiguration = ConfigurationFixture.GetFullConfigurationFixture();
        this.mockConfigurationService = new Mock<IConfigurationService>();
        this.mockConfigurationService.Setup(service => service.Configuration).Returns(this.mockConfiguration);
    }

    [Fact]
    public void StockSubscriptionServiceConstructor_ShouldCreateTimerProperly()
    {
        var service = this.CreateStockSubscriptionService();

        service.NotificationTimer.Interval.Should().Be(this.mockConfiguration.Notification.FetchInterval * 1000);
        service.NotificationTimer.AutoReset.Should().BeTrue();
        service.NotificationTimer.Enabled.Should().BeFalse();
    }

    [Fact]
    public void SubscribeToStock_ShouldCreateStockSubscriptionProperly()
    {
        var service = this.CreateStockSubscriptionService();

        var ticker = "PETR4";
        var stockReferenceValues = new StockReferenceValuesDto(20, 10);

        service.SubscribeToStock(ticker, stockReferenceValues);

        service.StockSubscription.Ticker.Should().Be(ticker);
        service.StockSubscription.StockReferenceValues.Should().BeEquivalentTo(stockReferenceValues);
    }

    [Fact]
    public void SubscribeToStock_ShouldStartTimer()
    {
        var service = this.CreateStockSubscriptionService();

        var ticker = "PETR4";
        var stockReferenceValues = new StockReferenceValuesDto(20, 10);

        service.SubscribeToStock(ticker, stockReferenceValues);

        service.NotificationTimer.Enabled.Should().BeTrue();
    }

    [Fact]
    public void OnNotificationTimerEvent_WhenShouldNotifyUser_ShouldSendMailNotification()
    {
        var service = this.CreateStockSubscriptionService();

        this.SetupPurchaseScenario(service);

        service.OnNotificationTimerEvent(null, null!);

        this.mockMailService.Verify(service => service.SendMail(It.IsAny<MailMessage>()));
    }

    [Fact]
    public async void GetSubscribedStockQuotes_ShouldCallBrapiApiWithSubscribedStockTicker()
    {
        var service = this.CreateStockSubscriptionService();

        var stockQuotes = this.SetupSaleScenario(service);

        var result = await service.GetSubscribedStockQuotes();

        result.Should().BeEquivalentTo(stockQuotes);
        this.mockBrapiService
            .Verify(
                s => s.GetStocksQuotesList(
                        It.Is<IEnumerable<string>>(tickers => tickers.Contains(service.StockSubscription.Ticker))
                    )
            );
    }

    [Theory]
    // sale scenario + last notification not sale = notify
    [InlineData(StockSubscriptionService.NotificationType.SALE, StockSubscriptionService.NotificationType.NONE, true)]
    // sale scenario + last notification sale = don't notify
    [InlineData(StockSubscriptionService.NotificationType.SALE, StockSubscriptionService.NotificationType.SALE, false)]
    // purchase scenario + last notification not purchase = notify
    [InlineData(StockSubscriptionService.NotificationType.PURCHASE, StockSubscriptionService.NotificationType.NONE, true)]
    // purchase scenario + last notification purchase = don't notify
    [InlineData(StockSubscriptionService.NotificationType.PURCHASE, StockSubscriptionService.NotificationType.PURCHASE, false)]
    public void ShouldNotifyUser_ShouldReturnWhenUserNeedsToBeNotified(
        StockSubscriptionService.NotificationType notificationType,
        StockSubscriptionService.NotificationType lastNotificationSentType,
        bool notify
    )
    {
        var service = this.CreateStockSubscriptionService();

        StockQuotesDto stockQuotes = null!;

        if (StockSubscriptionService.NotificationType.SALE.Equals(notificationType))
        {
            stockQuotes = this.SetupSaleScenario(service);
        }
        else if (StockSubscriptionService.NotificationType.PURCHASE.Equals(notificationType))
        {
            stockQuotes = this.SetupPurchaseScenario(service);
        }

        service.LastNotificationSentType = lastNotificationSentType;

        var result = service.ShouldNotifyUser(stockQuotes);

        result.Should().Be(notify);
    }

    [Fact]
    public void NotifyUser_ShouldSetLastNotificationSentTypeProperly()
    {
        var service = this.CreateStockSubscriptionService();

        var stockQuotes = this.SetupSaleScenario(service);

        service.NotifyUser(stockQuotes);

        service.LastNotificationSentType.Should().Be(StockSubscriptionService.NotificationType.SALE);
    }

    private StockSubscriptionService CreateStockSubscriptionService()
    {
        return new StockSubscriptionService(
            this.mockBrapiService.Object,
            this.mockMailService.Object,
            this.mockMailMessageService.Object,
            this.mockConfigurationService.Object
        );
    }

    private StockQuotesDto SetupSaleScenario(StockSubscriptionService service)
    {
        var ticker = "PETR4";
        var stockReferenceValues = new StockReferenceValuesDto(20, 10);

        service.StockSubscription = new StockSubscriptionDto()
        {
            Ticker = ticker,
            StockReferenceValues = stockReferenceValues
        };

        var stockQuotes = new StockQuotesDto()
        {
            Symbol = ticker,
            RegularMarketPrice = 25m
        };

        var stockQuotesList = new StocksQuotesListDto()
        {
            Results = new List<StockQuotesDto>() {
                stockQuotes
            }
        };

        this.mockBrapiService
            .Setup(service => service.GetStocksQuotesList(It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(stockQuotesList);

        return stockQuotes;
    }

    private StockQuotesDto SetupPurchaseScenario(StockSubscriptionService service)
    {
        var ticker = "PETR4";
        var stockReferenceValues = new StockReferenceValuesDto(20, 10);

        service.StockSubscription = new StockSubscriptionDto()
        {
            Ticker = ticker,
            StockReferenceValues = stockReferenceValues
        };

        var stockQuotes = new StockQuotesDto()
        {
            Symbol = ticker,
            RegularMarketPrice = 5m
        };

        var stockQuotesList = new StocksQuotesListDto()
        {
            Results = new List<StockQuotesDto>() {
                stockQuotes
            }
        };

        this.mockBrapiService
            .Setup(service => service.GetStocksQuotesList(It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(stockQuotesList);

        return stockQuotes;
    }
}
