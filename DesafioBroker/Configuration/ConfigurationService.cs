using Microsoft.Extensions.Configuration;

namespace DesafioBroker.Configuration;

public class ConfigurationService
{
    private readonly IConfiguration configurationInterface;

    public Models.Configuration? Configuration => this.configurationInterface.Get<Models.Configuration>();

    public ConfigurationService(IConfiguration configurationInterface)
    {
        this.configurationInterface = configurationInterface;
    }

}
