using System.Net.Mail;
using System.Threading.Tasks;
using Hackathon.Abstraction.User;
using Hackathon.BL.Email;
using Hackathon.Common.Configuration;
using Hackathon.Common.Models.User;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Hackathon.Tests.Integration;

public class EmailServiceTests: BaseIntegrationTest
{
    private IEmailService _service;

    public EmailServiceTests(TestWebApplicationFactory factory) : base(factory)
    {
        var appSettingsMock = new Mock<IOptions<AppSettings>>();
        appSettingsMock.Setup(x => x.Value)
            .Returns(new AppSettings
            {
                EmailSender = new EmailSenderSettings
                {
                    Sender = AppSettings.EmailSender.Sender
                }
            });

        var smtpClient = factory.Services.GetRequiredService<SmtpClient>();
        _service = new EmailService(appSettingsMock.Object, NullLogger<EmailService>.Instance, smtpClient);
    }

    [Fact]
    public async Task SendAsync_Test()
    {


        var result = await _service.SendAsync(new EmailParameters
        {
            Email = AppSettings.EmailSender.Sender,
            Subject = "subject",
            Body = "<b>OK</b>"
        });

        Assert.True(result.IsSuccess);
    }
}
