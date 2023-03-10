using System.Net;
using System.Net.Mail;
using DesafioBroker.Configuration.Interfaces;
using DesafioBroker.Mail.Interfaces;

namespace DesafioBroker.Mail.Services;

public class MailService : IMailService
{

    private readonly IConfigurationService configurationService;

    public MailService(IConfigurationService configurationService)
    {
        this.configurationService = configurationService;
    }

    public void SendMail(MailMessage message)
    {
        using var smtpClient = this.CreateSmtpClient();
        smtpClient.Send(message);
    }

    public virtual SmtpClient CreateSmtpClient()
    {
        var smtpConfig = this.configurationService.Configuration!.Email.SMTPConfig;

        return new SmtpClient()
        {
            Host = smtpConfig.Host,
            Port = smtpConfig.Port,
            EnableSsl = true,
            Credentials = new NetworkCredential(smtpConfig.Username, smtpConfig.Password)
        };
    }
}
