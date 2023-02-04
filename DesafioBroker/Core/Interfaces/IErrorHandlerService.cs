namespace DesafioBroker.Core.Interfaces;

public interface IErrorHandlerService
{
    void HandleError(Exception e);

    void HandleError(Exception e, string message);
}
