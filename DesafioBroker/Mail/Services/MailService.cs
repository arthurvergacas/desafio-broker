using System.Globalization;
using System.Net;
using System.Net.Mail;
using DesafioBroker.Configuration;
using DesafioBroker.Mail.Interfaces;

namespace DesafioBroker.Mail.Services;

public class MailService : IMailService
{

    private readonly ConfigurationService configurationService;

    public MailService(ConfigurationService configurationService)
    {
        this.configurationService = configurationService;
    }

    public void SendMail(MailMessage message)
    {
        using var smptClient = this.CreateSmtpClient();
        smptClient.Send(message);
    }

    private SmtpClient CreateSmtpClient()
    {
        var smtpConfig = this.configurationService.Configuration!.Email.SMTPConfig;

        return new SmtpClient()
        {
            Host = smtpConfig.Host,
            Port = int.Parse(smtpConfig.Port, new CultureInfo(CultureInfo.CurrentCulture.Name)),
            EnableSsl = true,
            Credentials = new NetworkCredential(smtpConfig.Username, smtpConfig.Password)
        };
    }
}