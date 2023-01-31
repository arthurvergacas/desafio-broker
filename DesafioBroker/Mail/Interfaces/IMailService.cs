using System.Net.Mail;

namespace DesafioBroker.Mail.Interfaces;

public interface IMailService
{
    void SendMail(MailMessage message);
}
