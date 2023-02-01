using Newtonsoft.Json;

namespace DesafioBroker.Brapi.Dtos;

public class StocksQuotesList
{
    public IEnumerable<StockQuotes> Results { get; set; } = null!;
    public DateTime RequestedAt { get; set; }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
