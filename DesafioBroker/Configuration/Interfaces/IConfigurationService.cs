namespace DesafioBroker.Configuration.Interfaces;

public interface IConfigurationService
{
    Models.Configuration Configuration { get; }

    public bool IsDevelopment();
}
