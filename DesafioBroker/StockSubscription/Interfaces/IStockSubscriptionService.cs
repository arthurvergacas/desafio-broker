using DesafioBroker.StockSubscription.Dtos;

namespace DesafioBroker.StockSubscription.Interfaces;

public interface IStockSubscriptionService : IDisposable
{
    void SubscribeToStock(StockSubscriptionDto stockSubscription);

}
