namespace DesafioBroker.Brapi.Dtos;

public class TickersQuotesList
{
    public IEnumerable<TickerQuotes> Results { get; set; } = null!;
    public DateTime RequestedAt { get; set; }

}
