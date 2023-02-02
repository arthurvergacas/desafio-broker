using System.Globalization;
using System.Net.Mail;
using DesafioBroker.Brapi.Dtos;
using DesafioBroker.Configuration.Interfaces;
using DesafioBroker.Dtos;
using DesafioBroker.Mail.Interfaces;

namespace DesafioBroker.Mail.Services;

public class MailMessageService : IMailMessageService
{

    private readonly IConfigurationService configurationService;

    public enum NotificationMode
    {
        SALE,
        PURCHASE
    }

    public MailMessageService(IConfigurationService configurationService)
    {
        this.configurationService = configurationService;
    }

    public MailMessage CreateNotificationMessage(StockReferenceValuesDto stockReferenceValues, StockQuotesDto stockQuotes)
    {
        if (stockQuotes.RegularMarketPrice > stockReferenceValues.SaleReferenceValue)
        {
            return this.CreateSaleNotificationMessage(stockReferenceValues, stockQuotes);
        }
        else if (stockQuotes.RegularMarketPrice < stockReferenceValues.PurchaseReferenceValue)
        {
            return this.CreatePurchaseNotificationMessage(stockReferenceValues, stockQuotes);
        }
        else
        {
            throw new ArgumentException(
                "Stock current price isn't in the specified range defined by the reference values to send a notification",
                nameof(stockQuotes)
            );
        }
    }

    public MailMessage CreateSaleNotificationMessage(StockReferenceValuesDto stockReferenceValues, StockQuotesDto stockQuotes)
    {
        var mail = this.CreateBaseNotificationMessage();

        mail.Subject = $"O preço da ação {stockQuotes.Symbol} subiu! 📈";

        mail.Body = CreateBaseNotificationBody(stockReferenceValues, stockQuotes, NotificationMode.SALE);

        return mail;
    }


    public MailMessage CreatePurchaseNotificationMessage(StockReferenceValuesDto stockReferenceValues, StockQuotesDto stockQuotes)
    {
        var mail = this.CreateBaseNotificationMessage();

        mail.Subject = $"O preço da ação {stockQuotes.Symbol} caiu! 📉";

        mail.Body = CreateBaseNotificationBody(stockReferenceValues, stockQuotes, NotificationMode.PURCHASE);

        return mail;
    }

    public MailMessage CreateBaseNotificationMessage()
    {
        var mail = new MailMessage
        {
            From = new MailAddress(this.configurationService.Configuration.Email.SMTPConfig.Sender),
            IsBodyHtml = true,
            BodyEncoding = System.Text.Encoding.UTF8,
        };

        mail.To.Add(this.configurationService.Configuration.Email.Recipient);

        return mail;
    }

    public static string CreateBaseNotificationBody(
        StockReferenceValuesDto stockReferenceValues,
        StockQuotesDto stockQuotes,
        NotificationMode mode
    )
    {

        var isSale = NotificationMode.SALE.Equals(mode);

        var difference =
            Math.Abs((isSale
                ? stockReferenceValues.SaleReferenceValue
                : stockReferenceValues.PurchaseReferenceValue
            ) - stockQuotes.RegularMarketPrice);

        return @$"
        <head>
            <style>
                * {{
                    font-family: sans-serif;
                }}

                p {{
                    font-size: 1.15rem;
                }}
            </style>
        </head>

        <body>
            <h1>{stockQuotes.Symbol} | {stockQuotes.LongName} {(isSale ? "subiu" : "caiu")} para R$ {FormatDecimalToCurrency(stockQuotes.RegularMarketPrice)}! {(isSale ? "📈" : "📉")}</h1>

            <p>
                O preço da ação <strong>{stockQuotes.Symbol} | {stockQuotes.LongName}</strong> {(isSale ? "subiu" : "caiu")} para <strong>R$ {FormatDecimalToCurrency(stockQuotes.RegularMarketPrice)}</strong>, R$ {FormatDecimalToCurrency(difference)} mais {(isSale ? "caro" : "barato")} que o valor de referência definido para {(isSale ? "venda" : "compra")}, <strong>R$ {FormatDecimalToCurrency(isSale ? stockReferenceValues.SaleReferenceValue : stockReferenceValues.PurchaseReferenceValue)}</strong>.
            </p>

            <p>Essa é uma boa hora para {(isSale ? "vender" : "comprar")} ações da empresa! 💵</p>
        </body>
        ";
    }

    private static string FormatDecimalToCurrency(decimal number)
    {
        return number.ToString("0.00", CultureInfo.CreateSpecificCulture("pt-br"));
    }

}
