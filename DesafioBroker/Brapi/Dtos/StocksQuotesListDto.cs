using Newtonsoft.Json;

namespace DesafioBroker.Brapi.Dtos;

public class StocksQuotesListDto
{
    public IList<StockQuotesDto> Results { get; set; } = null!;
    public DateTime RequestedAt { get; set; }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
