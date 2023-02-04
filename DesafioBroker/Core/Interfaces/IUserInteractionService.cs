using DesafioBroker.StockSubscription.Dtos;

namespace DesafioBroker.Core.Interfaces;

public interface IUserInteractionService
{
    StockSubscriptionDto ParseUserInput(string[] args);

    void WaitForUserCommands();
}
