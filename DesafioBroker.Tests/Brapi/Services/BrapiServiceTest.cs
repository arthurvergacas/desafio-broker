using Moq;
using DesafioBroker.Brapi.Interfaces;
using DesafioBroker.Brapi.Dtos;
using DesafioBroker.Brapi.Services;

namespace DesafioBroker.Tests.Brapi.Services;

public class BrapiServiceTest
{
    private readonly Mock<IBrapiClient> mockBrapiClient;
    private readonly BrapiService brapiService;

    public BrapiServiceTest()
    {
        this.mockBrapiClient = new Mock<IBrapiClient>();

        this.brapiService = new BrapiService(this.mockBrapiClient.Object);
    }

    [Fact]
    public async Task GetStocksQuotesList_WithSomeTickers_ShouldReturnClientResult()
    {
        var expectedStocksQuotes = new StocksQuotesList();

        this.mockBrapiClient
            .Setup(service => service.GetStocksQuotesList(It.IsAny<string>()))
            .ReturnsAsync(expectedStocksQuotes);

        var tickers = new List<string> { "PETR4", "VALE3" };

        var stocksQuotes = await this.brapiService.GetStocksQuotesList(tickers);

        stocksQuotes.Should().Be(expectedStocksQuotes);
    }

    [Fact]
    public async Task GetStocksQuotesList_WithSomeTickers_ShouldCallClientProperly()
    {
        this.mockBrapiClient
            .Setup(service => service.GetStocksQuotesList(It.IsAny<string>()));

        var tickers = new List<string> { "PETR4", "VALE3" };
        var parsedTickers = "PETR4,VALE3";

        await this.brapiService.GetStocksQuotesList(tickers);

        this.mockBrapiClient.Verify((service) => service.GetStocksQuotesList(parsedTickers));
    }

    [Fact]
    public async Task GetStocksQuotesList_WithNullTickers_ShouldThrowArgumentNullException()
    {
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            async () => await this.brapiService.GetStocksQuotesList(null!)
        );

        exception.ParamName.Should().Be("tickers");
    }

    [Fact]
    public async Task GetStocksQuotesList_WithNoTickers_ShouldThrowArgumentException()
    {
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            async () => await this.brapiService.GetStocksQuotesList(new List<string> { })
        );

        exception.Message.Should().Contain("List of tickers cannot be empty");
        exception.ParamName.Should().Be("tickers");
    }

}
