using DesafioBroker.Configuration.Models;

namespace DesafioBroker.Tests.Fixtures;

public class ConfigurationFixture
{
    public static Configuration.Models.Configuration GetFullConfigurationFixture()
    {
        return new Configuration.Models.Configuration()
        {
            Stock = new Stock()
            {
                Brapi = new Configuration.Models.Brapi()
                {
                    QuotesUrl = "https://mock/api/quote/"
                }
            },

            Notification = new Notification()
            {
                FetchInterval = 60
            },

            Email = new Email()
            {
                SMTPConfig = new SmtpConfig
                {
                    Sender = "sender@mock.com",
                    Host = "mockHost",
                    Port = 123,
                    Username = "mockUsername",
                    Password = "mockPassword"
                },
                Recipient = "recipient@mock.com"
            }
        };
    }
}
