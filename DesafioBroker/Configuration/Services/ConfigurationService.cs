using DesafioBroker.Configuration.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DesafioBroker.Configuration.Services;

public class ConfigurationService : IConfigurationService
{
    private readonly IConfiguration configurationInterface;

    public Models.Configuration Configuration => this.configurationInterface.Get<Models.Configuration>()!;

    public ConfigurationService(IConfiguration configurationInterface)
    {
        this.configurationInterface = configurationInterface;
    }

}
