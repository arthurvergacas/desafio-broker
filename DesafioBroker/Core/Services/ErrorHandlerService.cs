using DesafioBroker.Core.Interfaces;
using Microsoft.Extensions.Hosting;

namespace DesafioBroker.Core.Services;

public class ErrorHandlerService : IErrorHandlerService
{
    private readonly IHostApplicationLifetime applicationLifetime;

    public ErrorHandlerService(IHostApplicationLifetime applicationLifetime)
    {
        this.applicationLifetime = applicationLifetime;
    }

    public void HandleError(Exception e, string? message)
    {
        Console.WriteLine(message ?? e.Message);

        this.ExitApplication();
    }

    private void ExitApplication()
    {
        this.applicationLifetime.StopApplication();
    }
}
