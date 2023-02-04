using System.Net.Mail;
using DesafioBroker.Brapi.Dtos;
using DesafioBroker.Dtos;

namespace DesafioBroker.Mail.Interfaces;

public interface IMailMessageService
{
    MailMessage CreateNotificationMessage(StockReferenceValuesDto stockReferenceValues, StockQuotesDto stockQuotes);
}
