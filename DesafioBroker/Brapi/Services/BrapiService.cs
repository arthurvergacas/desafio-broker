using System.Net;
using DesafioBroker.Brapi.Dtos;
using DesafioBroker.Brapi.Interfaces;
using DesafioBroker.Core.Interfaces;

namespace DesafioBroker.Brapi.Services;

public class BrapiService : IBrapiService
{
    private readonly IBrapiClient brapiClient;

    private readonly IErrorHandlerService errorHandlerService;

    public BrapiService(IBrapiClient brapiClient, IErrorHandlerService errorHandlerService)
    {
        this.brapiClient = brapiClient;
        this.errorHandlerService = errorHandlerService;
    }

    public async Task<StocksQuotesListDto> GetStocksQuotesList(IList<string> tickers)
    {
        ArgumentNullException.ThrowIfNull(tickers, nameof(tickers));

        if (!tickers.Any())
        {
            throw new ArgumentException("List of tickers cannot be empty", nameof(tickers));
        }

        try
        {
            return await this.brapiClient.GetStocksQuotesList(string.Join(',', tickers));
        }
        catch (HttpRequestException e)
        {
            if (e.StatusCode == HttpStatusCode.NotFound)
            {
                this.errorHandlerService.HandleError(e, $"Ticker '{tickers[0]}' could not be found.");
            }
            else
            {
                this.errorHandlerService.HandleError(e, "An error occured while fetching the stock data.");
            }

            return new StocksQuotesListDto();
        }
    }
}
