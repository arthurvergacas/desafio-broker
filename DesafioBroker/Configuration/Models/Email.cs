namespace DesafioBroker.Configuration.Models;

public class Email
{
    public string Recipient { get; init; } = null!;
    public SMTPConfig SMTPConfig { get; init; } = null!;

}
