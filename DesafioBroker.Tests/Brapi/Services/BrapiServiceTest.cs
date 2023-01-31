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
    public async Task GetTickersQuotesList_WithSomeTickers_ShouldReturnClientResult()
    {
        var expectedTickersQuotes = new TickersQuotesList();

        this.mockBrapiClient
            .Setup(service => service.GetTickersQuotesList(It.IsAny<string>()))
            .ReturnsAsync(expectedTickersQuotes);

        var tickers = new List<string> { "PETR4", "VALE3" };

        var tickersQuotes = await this.brapiService.GetTickersQuotesList(tickers);

        tickersQuotes.Should().Be(expectedTickersQuotes);
    }

    [Fact]
    public async Task GetTickersQuotesList_WithSomeTickers_ShouldCallClientProperly()
    {
        this.mockBrapiClient
            .Setup(service => service.GetTickersQuotesList(It.IsAny<string>()));

        var tickers = new List<string> { "PETR4", "VALE3" };
        var parsedTickers = "PETR4,VALE3";

        await this.brapiService.GetTickersQuotesList(tickers);

        this.mockBrapiClient.Verify((service) => service.GetTickersQuotesList(parsedTickers));
    }

    [Fact]
    public async Task GetTickersQuotesList_WithNullTickers_ShouldThrowArgumentNullException()
    {
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            async () => await this.brapiService.GetTickersQuotesList(null!)
        );

        exception.ParamName.Should().Be("tickers");
    }

    [Fact]
    public async Task GetTickersQuotesList_WithNoTickers_ShouldThrowArgumentException()
    {
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            async () => await this.brapiService.GetTickersQuotesList(new List<string> { })
        );

        exception.Message.Should().Contain("List of tickers cannot be empty");
        exception.ParamName.Should().Be("tickers");
    }

}
