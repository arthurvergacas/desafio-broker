using System.Net.Mail;
using DesafioBroker.Brapi.Dtos;
using DesafioBroker.Dtos;

namespace DesafioBroker.Mail.Interfaces;

public interface IMailMessageService
{
    MailMessage CreateNotificationMessage(StockReferenceValues stockReferenceValues, StockQuotes stockQuotes);
}
