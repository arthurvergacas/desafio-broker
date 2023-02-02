using System.Net.Mail;
using DesafioBroker.Brapi.Dtos;
using DesafioBroker.Configuration.Interfaces;
using DesafioBroker.Configuration.Models;
using DesafioBroker.Dtos;
using DesafioBroker.Mail.Services;
using Moq;

namespace DesafioBroker.Tests.Mail.Services;

public class MailMessageServiceTest
{
    private readonly Mock<IConfigurationService> mockConfigurationService;
    private readonly Configuration.Models.Configuration mockConfiguration;
    private readonly StockQuotesDto mockStockQuotes;
    private readonly MailMessageService mailMessageService;

    public MailMessageServiceTest()
    {
        this.mockConfiguration = new Configuration.Models.Configuration()
        {
            Email = new Email()
            {
                SMTPConfig = new SmtpConfig
                {
                    Sender = "sender@mock.com"
                },
                Recipient = "recipient@mock.com"
            }
        };

        this.mockConfigurationService = new Mock<IConfigurationService>();
        this.mockConfigurationService.Setup(service => service.Configuration).Returns(this.mockConfiguration);

        this.mockStockQuotes = new StockQuotesDto()
        {
            Symbol = "MOCK",
            LongName = "Mock Stock",
        };

        this.mailMessageService = new MailMessageService(this.mockConfigurationService.Object);
    }

    [Fact]
    public void CreateNotificationMessage_WithQuotesHigherThanSaleReference_ShouldReturnSaleNotification()
    {
        decimal saleReferenceValue = 12;
        decimal purchaseReferenceValue = 10;

        var stockReferenceValues = new StockReferenceValuesDto(saleReferenceValue, purchaseReferenceValue);

        this.mockStockQuotes.RegularMarketPrice = saleReferenceValue + 2;

        var mailMessage = this.mailMessageService.CreateNotificationMessage(stockReferenceValues, this.mockStockQuotes);

        mailMessage.Subject.Should().Contain($"O preço da ação {this.mockStockQuotes.Symbol} subiu! 📈");
    }

    [Fact]
    public void CreateNotificationMessage_WithQuotesLowerThanPurchaseReference_ShouldReturnPurchaseNotification()
    {
        decimal saleReferenceValue = 12;
        decimal purchaseReferenceValue = 10;

        var stockReferenceValues = new StockReferenceValuesDto(saleReferenceValue, purchaseReferenceValue);

        this.mockStockQuotes.RegularMarketPrice = purchaseReferenceValue - 2;

        var mailMessage = this.mailMessageService.CreateNotificationMessage(stockReferenceValues, this.mockStockQuotes);

        mailMessage.Subject.Should().Contain($"O preço da ação {this.mockStockQuotes.Symbol} caiu! 📉");
    }

    [Fact]
    public void CreateNotificationMessage_WithQuotesBetweenReferenceValues_ShouldThrowAnException()
    {
        decimal saleReferenceValue = 12;
        decimal purchaseReferenceValue = 10;

        var stockReferenceValues = new StockReferenceValuesDto(saleReferenceValue, purchaseReferenceValue);

        this.mockStockQuotes.RegularMarketPrice = (saleReferenceValue + purchaseReferenceValue) / 2;

        var createNotificationMessageAction =
            () => this.mailMessageService.CreateNotificationMessage(stockReferenceValues, this.mockStockQuotes);

        createNotificationMessageAction.Should()
            .Throw<ArgumentException>()
            .WithMessage(
                "Stock current price isn't in the specified range defined by the reference values to send a notification*"
            )
            .WithParameterName("stockQuotes");
    }

    [Fact]
    public void CreateSaleNotificationMessage_ShouldCreateSaleNotificationMessageProperly()
    {
        decimal saleReferenceValue = 12;
        decimal purchaseReferenceValue = 10;

        var stockReferenceValues = new StockReferenceValuesDto(saleReferenceValue, purchaseReferenceValue);

        this.mockStockQuotes.RegularMarketPrice = saleReferenceValue + 2;

        var mailMessage = this.mailMessageService
            .CreateSaleNotificationMessage(stockReferenceValues, this.mockStockQuotes);

        mailMessage.Body.Should().Contain(
            $"{this.mockStockQuotes.Symbol} | {this.mockStockQuotes.LongName} subiu para R$ 14,00! 📈"
        );
    }


    [Fact]
    public void CreatePurchaseNotificationMessage_ShouldCreatePurchaseNotificationMessageProperly()
    {
        decimal saleReferenceValue = 12;
        decimal purchaseReferenceValue = 10;

        var stockReferenceValues = new StockReferenceValuesDto(saleReferenceValue, purchaseReferenceValue);

        this.mockStockQuotes.RegularMarketPrice = purchaseReferenceValue - 2;

        var mailMessage = this.mailMessageService
            .CreatePurchaseNotificationMessage(stockReferenceValues, this.mockStockQuotes);

        mailMessage.Body.Should().Contain(
            $"{this.mockStockQuotes.Symbol} | {this.mockStockQuotes.LongName} caiu para R$ 8,00! 📉"
        );
    }

    [Fact]
    public void CreateBaseNotificationMessage_ShouldCreatePurchaseNotificationMessageProperly()
    {
        var mailMessage = this.mailMessageService.CreateBaseNotificationMessage();

        mailMessage.From.Should().BeEquivalentTo(new MailAddress(this.mockConfiguration.Email.SMTPConfig.Sender));
        mailMessage.IsBodyHtml.Should().BeTrue();
        mailMessage.BodyEncoding.Should().BeEquivalentTo(System.Text.Encoding.UTF8);

        var toAddressCollection = new MailAddressCollection
        {
            this.mockConfiguration.Email.Recipient
        };

        mailMessage.To.Should().BeEquivalentTo(toAddressCollection);
    }
}
