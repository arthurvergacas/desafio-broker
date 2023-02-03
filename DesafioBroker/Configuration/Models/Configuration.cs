namespace DesafioBroker.Configuration.Models;

public class Configuration
{
    public Stock Stock { get; init; } = null!;
    public Email Email { get; init; } = null!;
    public Notification Notification { get; init; } = null!;
}
