using DesafioBroker.StockSubscription.Dtos;

namespace DesafioBroker.Core.Interfaces;

public interface IUserInputService
{
    StockSubscriptionDto ParseUserInput(string[] args);

    void WaitForUserCommands();
}
