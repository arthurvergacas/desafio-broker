using Microsoft.Extensions.Configuration;

namespace DesafioBroker.Configuration;

public class ConfigurationManager
{
    public static Models.Configuration? Configuration => GetConfigurationInterface().Get<Models.Configuration>();

    private static IConfiguration? configurationInterface;

    private ConfigurationManager() { }

    private static IConfiguration GetConfigurationInterface()
    {
        if (configurationInterface == null)
        {
            configurationInterface = BuildConfiguration();
            return configurationInterface;
        }

        return configurationInterface;
    }

    private static IConfiguration BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
