namespace DesafioBroker.Brapi.Dtos;

public class StockQuotes
{
    public string Symbol { get; set; } = null!;
    public string ShortName { get; set; } = null!;
    public string LongName { get; set; } = null!;
    public string Currency { get; set; } = null!;
    public decimal RegularMarketPrice { get; set; }
    public decimal RegularMarketDayHigh { get; set; }
    public decimal RegularMarketDayLow { get; set; }

}
