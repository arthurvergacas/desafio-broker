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
    public async Task GetTickersQuotesList_WithSomeTickers_ShouldCallClientProperly()
    {
        var expectedTickersQuotes = new TickersQuotesList();

        this.mockBrapiClient
            .Setup(service => service.GetTickersQuotesList(It.IsAny<string>()))
            .ReturnsAsync(expectedTickersQuotes);

        var tickers = new List<string> { "PETR4", "VALE3" };
        var parsedTickers = "PETR4,VALE3";

        var tickersQuotes = await this.brapiService.GetTickersQuotesList(tickers);

        Assert.Equal(expectedTickersQuotes, tickersQuotes);
        this.mockBrapiClient.Verify((service) => service.GetTickersQuotesList(parsedTickers));
    }

    [Fact]
    public async Task GetTickersQuotesList_WithSomeTickers_ShouldReturnClientResult()
    {
        var expectedTickersQuotes = new TickersQuotesList();

        this.mockBrapiClient
            .Setup(service => service.GetTickersQuotesList(It.IsAny<string>()))
            .ReturnsAsync(expectedTickersQuotes);

        var tickers = new List<string> { "PETR4", "VALE3" };

        var tickersQuotes = await this.brapiService.GetTickersQuotesList(tickers);

        Assert.Equal(expectedTickersQuotes, tickersQuotes);
    }

}