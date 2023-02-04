using DesafioBroker.Core.Services;
using Microsoft.Extensions.Hosting;
using Moq;

namespace DesafioBroker.Tests.Core.Services;

public class UserInteractionServiceTest
{

    private readonly Mock<IHostApplicationLifetime> mockHostApplicationLifetime;

    public UserInteractionServiceTest()
    {
        this.mockHostApplicationLifetime = new Mock<IHostApplicationLifetime>();
    }

    [Fact]
    public void ParseUserInput_WithCorrectArgs_ShouldReturnStockSubscription()
    {
        var service = new UserInteractionService(this.mockHostApplicationLifetime.Object);

        var ticker = "PETR4";

        var decimalPurchaseReference = 4.34m;
        var purchaseReference = "4.34";

        var decimalSaleReference = 13.21m;
        var saleReference = "13.21";

        var stockSubscription = service.ParseUserInput(
            new string[] { ticker, purchaseReference, saleReference }
        );

        stockSubscription.Ticker.Should().Be(ticker);
        stockSubscription.StockReferenceValues.PurchaseReferenceValue.Should().Be(decimalPurchaseReference);
        stockSubscription.StockReferenceValues.SaleReferenceValue.Should().Be(decimalSaleReference);
    }

    [Fact]
    public void ParseUserInput_WithIncorrectNumberOfArgs_ShouldThrowAnException()
    {
        var service = new UserInteractionService(this.mockHostApplicationLifetime.Object);

        var ticker = "PETR4";

        var parseUserAction = () => service.ParseUserInput(
            new string[] { ticker }
        );

        parseUserAction.Should()
            .Throw<ArgumentException>()
            .WithMessage(
                "Arguments not passed correctly. The syntax is '<PROGRAM> <TICKER> <PURCHASE_REFERENCE> <SALE_REFERENCE>'"
            );
    }

    [Theory]
    [InlineData("23.11.2", "a45")]
    [InlineData("23. 11", "a 45")]
    [InlineData("23,1a1", "45.,1")]
    public void ParseUserInput_WithMalformedNumbers_ShouldThrowAnException(string purchaseReference, string saleReference)
    {
        var service = new UserInteractionService(this.mockHostApplicationLifetime.Object);

        var ticker = "PETR4";

        var parseUserAction = () => service.ParseUserInput(
           new string[] { ticker, purchaseReference, saleReference }
       );

        parseUserAction.Should()
            .Throw<ArgumentException>()
            .WithMessage("Unable to parse reference values to a number");
    }

    [Theory]
    [InlineData("   PETR4", "23.2     ", "    34.231      ")]
    [InlineData("   PETR4   ", " 23,2     ", "    34,231     ")]
    public void ParseUserInput_WithUnformattedArgs_ShouldNotThrowAnException(
        string ticker,
        string purchaseReference,
        string saleReference
    )
    {
        var service = new UserInteractionService(this.mockHostApplicationLifetime.Object);

        var parseUserAction = () => service.ParseUserInput(
           new string[] { ticker, purchaseReference, saleReference }
       );

        parseUserAction.Should().NotThrow();
    }

}
