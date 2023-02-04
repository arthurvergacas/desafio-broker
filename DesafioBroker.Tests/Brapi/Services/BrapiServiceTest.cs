using Moq;
using DesafioBroker.Core.Interfaces;
using DesafioBroker.Brapi.Interfaces;
using DesafioBroker.Brapi.Dtos;
using DesafioBroker.Brapi.Services;

namespace DesafioBroker.Tests.Brapi.Services;

public class BrapiServiceTest
{
    private readonly Mock<IBrapiClient> mockBrapiClient;

    private readonly Mock<IErrorHandlerService> mockErrorHandlerService;

    public BrapiServiceTest()
    {
        this.mockBrapiClient = new Mock<IBrapiClient>();

        this.mockErrorHandlerService = new Mock<IErrorHandlerService>();
    }

    [Fact]
    public async Task GetStocksQuotesList_WithSomeTickers_ShouldReturnClientResult()
    {
        var service = new BrapiService(this.mockBrapiClient.Object, this.mockErrorHandlerService.Object);

        var expectedStocksQuotes = new StocksQuotesListDto();

        this.mockBrapiClient
            .Setup(service => service.GetStocksQuotesList(It.IsAny<string>()))
            .ReturnsAsync(expectedStocksQuotes);

        var tickers = new List<string> { "PETR4", "VALE3" };

        var stocksQuotes = await service.GetStocksQuotesList(tickers);

        stocksQuotes.Should().Be(expectedStocksQuotes);
    }

    [Fact]
    public async Task GetStocksQuotesList_WithSomeTickers_ShouldCallClientProperly()
    {
        var service = new BrapiService(this.mockBrapiClient.Object, this.mockErrorHandlerService.Object);

        this.mockBrapiClient
            .Setup(service => service.GetStocksQuotesList(It.IsAny<string>()));

        var tickers = new List<string> { "PETR4", "VALE3" };
        var parsedTickers = "PETR4,VALE3";

        await service.GetStocksQuotesList(tickers);

        this.mockBrapiClient.Verify((service) => service.GetStocksQuotesList(parsedTickers));
    }

    [Fact]
    public async Task GetStocksQuotesList_WithNullTickers_ShouldThrowArgumentNullException()
    {
        var service = new BrapiService(this.mockBrapiClient.Object, this.mockErrorHandlerService.Object);

        var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            async () => await service.GetStocksQuotesList(null!)
        );

        exception.ParamName.Should().Be("tickers");
    }

    [Fact]
    public async Task GetStocksQuotesList_WithNoTickers_ShouldThrowArgumentException()
    {
        var service = new BrapiService(this.mockBrapiClient.Object, this.mockErrorHandlerService.Object);

        var exception = await Assert.ThrowsAsync<ArgumentException>(
            async () => await service.GetStocksQuotesList(new List<string> { })
        );

        exception.Message.Should().Contain("List of tickers cannot be empty");
        exception.ParamName.Should().Be("tickers");
    }

}
