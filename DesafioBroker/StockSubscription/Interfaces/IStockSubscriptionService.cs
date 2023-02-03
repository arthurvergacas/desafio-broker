using DesafioBroker.StockSubscription.Dtos;

namespace DesafioBroker.StockSubscription.Interfaces;

public interface IStockSubscriptionService
{
    void SubscribeToStock(StockSubscriptionDto stockSubscription);

}
