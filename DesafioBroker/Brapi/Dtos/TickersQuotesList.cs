namespace DesafioBroker.Brapi.Dtos;

public class TickersQuotesList
{
    public TickerQuotes[] Results { get; set; } = null!;
    public DateTime RequestedAt { get; set; }

}
