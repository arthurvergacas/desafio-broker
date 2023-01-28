namespace DesafioBroker.Configuration.Models;

public class Email
{
    public string? Recipient { get; init; }
    public SMTPConfig? SMTPConfig { get; init; }

}
