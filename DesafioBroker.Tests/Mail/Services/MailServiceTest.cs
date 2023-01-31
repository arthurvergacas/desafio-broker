using System.Globalization;
using System.Net;
using DesafioBroker.Configuration.Interfaces;
using DesafioBroker.Configuration.Models;
using DesafioBroker.Mail.Services;
using Moq;

namespace DesafioBroker.Tests.Mail.Services;

public class MailServiceTest
{

    private readonly Mock<IConfigurationService> mockConfigurationService;
    private readonly Configuration.Models.Configuration mockConfiguration;

    public MailServiceTest()
    {
        this.mockConfiguration = new Configuration.Models.Configuration()
        {
            Email = new Email()
            {
                SMTPConfig = new SmtpConfig
                {
                    Host = "mockHost",
                    Port = "123",
                    Username = "mockUsername",
                    Password = "mockPassword"
                }
            }
        };

        this.mockConfigurationService = new Mock<IConfigurationService>();
        this.mockConfigurationService.Setup(service => service.Configuration).Returns(this.mockConfiguration);
    }

    [Fact]
    public void CreateSmtpClient_ShouldCreateClientProperly()
    {
        var mailService = new MailService(this.mockConfigurationService.Object);

        var smtpClient = mailService.CreateSmtpClient();

        var smtpConfig = this.mockConfiguration.Email.SMTPConfig;

        smtpClient.Host.Should().Be(smtpConfig.Host);
        smtpClient.Port.ToString(new CultureInfo(CultureInfo.CurrentCulture.Name)).Should().Be(smtpConfig.Port);
        smtpClient.Credentials.Should().BeEquivalentTo(new NetworkCredential(smtpConfig.Username, smtpConfig.Password));
    }
}
