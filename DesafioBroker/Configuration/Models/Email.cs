namespace DesafioBroker.Configuration.Models;

public class Email
{
    public string Recipient { get; init; } = null!;
    public SmtpConfig SMTPConfig { get; init; } = null!;

}
