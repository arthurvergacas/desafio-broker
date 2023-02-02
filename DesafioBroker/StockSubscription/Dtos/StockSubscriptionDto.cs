using DesafioBroker.Dtos;

namespace DesafioBroker.StockSubscription.Dtos;

public class StockSubscriptionDto
{
    public string Ticker { get; set; } = null!;
    public StockReferenceValuesDto StockReferenceValues { get; set; } = null!;
}
