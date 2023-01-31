namespace DesafioBroker.Configuration.Models;

public class SmtpConfig
{
    public string Host { get; init; } = null!;
    public string Port { get; init; } = null!;
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
    public string Sender { get; init; } = null!;
}
