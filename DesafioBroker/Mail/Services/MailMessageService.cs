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

    public MailMessageService(IConfigurationService configurationService)
    {
        this.configurationService = configurationService;
    }
    public MailMessage CreateNotificationMessage(StockReferenceValues stockReferenceValues, StockQuotes stockQuotes)
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

    public MailMessage CreateSaleNotificationMessage(StockReferenceValues stockReferenceValues, StockQuotes stockQuotes)
    {
        var mail = this.CreateBaseNotificationMessage();

        mail.Subject = $"O preço da ação {stockQuotes.Symbol} subiu! 📈";

        mail.Body = CreateBaseNotificationBody(stockReferenceValues, stockQuotes, true);

        return mail;
    }


    public MailMessage CreatePurchaseNotificationMessage(StockReferenceValues stockReferenceValues, StockQuotes stockQuotes)
    {
        var mail = this.CreateBaseNotificationMessage();

        mail.Subject = $"O preço da ação {stockQuotes.Symbol} caiu! 📉";

        mail.Body = CreateBaseNotificationBody(stockReferenceValues, stockQuotes, false);

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
        StockReferenceValues stockReferenceValues,
        StockQuotes stockQuotes,
        bool isSale
    )
    {
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
        </style>
        </head>

        <h1>{stockQuotes.Symbol} | {stockQuotes.LongName} {(isSale ? "subiu" : "caiu")} para R$ {FormatDecimalToCurrency(stockQuotes.RegularMarketPrice)}! {(isSale ? "📈" : "📉")}</h1>

        <p>
        O preço da ação <strong>{stockQuotes.Symbol} | {stockQuotes.LongName}</strong> {(isSale ? "subiu" : "caiu")} para <strong>R$ {FormatDecimalToCurrency(stockQuotes.RegularMarketPrice)}</strong>, R$ {FormatDecimalToCurrency(difference)} mais {(isSale ? "caro" : "barato")} que o valor de referência definido para {(isSale ? "venda" : "compra")}, <strong>R$ {FormatDecimalToCurrency(isSale ? stockReferenceValues.SaleReferenceValue : stockReferenceValues.PurchaseReferenceValue)}</strong>.
        </p>

        <p>Essa é uma boa hora para {(isSale ? "vender" : "comprar")} ações da empresa!</p>";
    }

    private static string FormatDecimalToCurrency(decimal number)
    {
        return number.ToString("0.00", CultureInfo.CreateSpecificCulture("pt-br"));
    }

}
