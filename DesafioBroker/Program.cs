using Microsoft.Extensions.DependencyInjection;
using DesafioBroker.Core.Interfaces;

namespace DesafioBroker;

public class Program
{
    public static void Main(string[] args)
    {
        var host = HostManager.GetGenericHost();

        using var serviceScope = host.Services.CreateScope();

        var services = serviceScope.ServiceProvider;

        services.GetService<IDesafioBrokerCoreService>()!.Run(args);
    }
}

