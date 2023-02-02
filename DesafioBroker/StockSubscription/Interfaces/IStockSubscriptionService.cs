using DesafioBroker.Dtos;

namespace DesafioBroker.StockSubscription.Interfaces;

public interface IStockSubscriptionService
{
    void SubscribeToStock(string ticker, StockReferenceValuesDto stockReferenceValues);
}
